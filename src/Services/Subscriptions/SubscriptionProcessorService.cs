﻿namespace WhMgr.Services.Subscriptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using DSharpPlus;
    using DSharpPlus.Entities;
    using Microsoft.Extensions.Logging;

    using WhMgr.Configuration;
    using WhMgr.Data;
    using WhMgr.Extensions;
    using WhMgr.Localization;
    using WhMgr.Queues;
    using WhMgr.Services.Alarms;
    using WhMgr.Services.Alarms.Filters;
    using WhMgr.Services.Geofence;
    using WhMgr.Services.Subscriptions.Models;
    using WhMgr.Services.Webhook.Models;

    public class SubscriptionProcessorService : ISubscriptionProcessorService
    {
        private readonly ILogger<ISubscriptionProcessorService> _logger;
        private readonly ISubscriptionManagerService _subscriptionManager;
        private readonly ConfigHolder _config;
        private readonly Dictionary<ulong, DiscordClient> _discordClients;
        private readonly NotificationQueue _queue;

        public SubscriptionProcessorService(
            ILogger<ISubscriptionProcessorService> logger,
            ISubscriptionManagerService subscriptionManager,
            ConfigHolder config,
            Dictionary<ulong, DiscordClient> discordClients,
            NotificationQueue queue)
        {
            _logger = logger;
            _subscriptionManager = subscriptionManager;
            _config = config;
            _discordClients = discordClients;
            _queue = queue;
        }

        public async Task ProcessPokemonSubscription(PokemonData pokemon)
        {
            if (!MasterFile.Instance.Pokedex.ContainsKey(pokemon.Id))
                return;

            // Cache the result per-guild so that geospatial stuff isn't queried for every single subscription below
            var locationCache = new Dictionary<ulong, Geofence>();
            Geofence GetGeofence(ulong guildId)
            {
                if (!locationCache.TryGetValue(guildId, out var geofence))
                {
                    var geofences = _config.Instance.Servers[guildId].Geofences;
                    geofence = GeofenceService.GetGeofence(geofences, new Coordinate(pokemon.Latitude, pokemon.Longitude));
                    locationCache.Add(guildId, geofence);
                }

                return geofence;
            }

            var subscriptions = await _subscriptionManager.GetSubscriptionsByPokemonId(pokemon.Id);
            if (subscriptions == null)
            {
                _logger.LogWarning($"Failed to get subscriptions from database table.");
                return;
            }

            Subscription user;
            DiscordMember member = null;
            var pkmn = MasterFile.GetPokemon(pokemon.Id, pokemon.FormId);
            var matchesIV = false;
            var matchesLvl = false;
            var matchesGender = false;
            var matchesIVList = false;
            for (var i = 0; i < subscriptions.Count; i++)
            {
                //var start = DateTime.Now;
                try
                {
                    user = subscriptions[i];

                    if (!_config.Instance.Servers.ContainsKey(user.GuildId))
                        continue;

                    if (!_config.Instance.Servers[user.GuildId].Subscriptions.Enabled)
                        continue;

                    if (!_discordClients.ContainsKey(user.GuildId))
                        continue;

                    var client = _discordClients[user.GuildId];

                    try
                    {
                        member = await client.GetMemberById(user.GuildId, user.UserId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogDebug($"FAILED TO GET MEMBER BY ID {user.UserId}");
                        _logger.LogError(ex.ToString());
                        continue;
                    }

                    if (member?.Roles == null)
                        continue;

                    if (!member.HasSupporterRole(_config.Instance.Servers[user.GuildId].DonorRoleIds))
                    {
                        _logger.LogDebug($"User {member?.Username} ({user.UserId}) is not a supporter, skipping pokemon {pkmn.Name}...");
                        // Automatically disable users subscriptions if not supporter to prevent issues
                        //user.Enabled = false;
                        //user.Save(false);
                        continue;
                    }

                    var form = Translator.Instance.GetFormName(pokemon.FormId);
                    var pokemonSubscriptions = user.Pokemon.Where(x =>
                        x.PokemonId.Contains(pokemon.Id) && (x.Forms?.Contains(form) ?? true)
                    );
                    foreach (var pkmnSub in pokemonSubscriptions)
                    {
                        matchesIV = Filters.MatchesIV(pokemon.IV, (uint)pkmnSub.MinimumIV, 100);
                        //var matchesCP = _whm.Filters.MatchesCpFilter(pkmn.CP, subscribedPokemon.MinimumCP);
                        matchesLvl = Filters.MatchesLvl(pokemon.Level, (uint)pkmnSub.MinimumLevel, (uint)pkmnSub.MaximumLevel);
                        matchesGender = Filters.MatchesGender(pokemon.Gender, pkmnSub.Gender);
                        matchesIVList = pkmnSub.IVList?.Select(x => x.Replace("\r", null)).Contains($"{pokemon.Attack}/{pokemon.Defense}/{pokemon.Stamina}") ?? false;

                        if (!(
                            (!pkmnSub.HasStats && matchesIV && matchesLvl && matchesGender) ||
                            (pkmnSub.HasStats && matchesIVList)
                            ))
                            continue;

                        /*
                        TODO: if (!(float.TryParse(pokemon.Height, out var height) && float.TryParse(pokemon.Weight, out var weight) && Filters.MatchesSize(pokemon.Id.GetSize(height, weight), pkmnSub.Size)))
                        {
                            // Pokemon doesn't match size
                            continue;
                        }
                        */

                        var geofence = GetGeofence(user.GuildId);
                        if (geofence == null)
                        {
                            //_logger.Warn($"Failed to lookup city from coordinates {pkmn.Latitude},{pkmn.Longitude} {db.Pokemon[pkmn.Id].Name} {pkmn.IV}, skipping...");
                            continue;
                        }

                        var globalLocation = user.Locations?.FirstOrDefault(x => string.Compare(x.Name, user.Location, true) == 0);
                        var subscriptionLocation = user.Locations?.FirstOrDefault(x => string.Compare(x.Name, pkmnSub.Location, true) == 0);
                        var globalDistanceMatches = globalLocation?.DistanceM > 0 && globalLocation?.DistanceM > new Coordinate(globalLocation?.Latitude ?? 0, globalLocation?.Longitude ?? 0).DistanceTo(new Coordinate(pokemon.Latitude, pokemon.Longitude));
                        var subscriptionDistanceMatches = subscriptionLocation?.DistanceM > 0 && subscriptionLocation?.DistanceM > new Coordinate(subscriptionLocation?.Latitude ?? 0, subscriptionLocation?.Longitude ?? 0).DistanceTo(new Coordinate(pokemon.Latitude, pokemon.Longitude));
                        var geofenceMatches = pkmnSub.Areas.Select(x => x.ToLower()).Contains(geofence.Name.ToLower());

                        // If set distance does not match and no geofences match, then skip Pokemon...
                        if (!globalDistanceMatches && !subscriptionDistanceMatches && !geofenceMatches)
                            continue;

                        var embed = pokemon.GenerateEmbedMessage(new AlarmMessageSettings
                        {
                            GuildId = user.GuildId,
                            Client = client,
                            Config = _config,
                            Alarm = null,
                            City = geofence.Name,
                        });
                        //var end = DateTime.Now.Subtract(start);
                        //_logger.Debug($"Took {end} to process Pokemon subscription for user {user.UserId}");
                        embed.Embeds.ForEach(x => _queue.Enqueue(new NotificationItem
                        {
                            Subscription = user,
                            Member = member,
                            Embed = x,
                            Description = pkmn.Name,
                            City = geofence.Name,
                            Pokemon = pokemon,
                        }));

                        // TODO: Statistics.Instance.SubscriptionPokemonSent++;
                        Thread.Sleep(5);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                }
            }

            subscriptions.Clear();
            subscriptions = null;
            member = null;
            user = null;
            pokemon = null;

            await Task.CompletedTask;
        }

        public async Task ProcessPvpSubscription(PokemonData pokemon)
        {
            if (!MasterFile.Instance.Pokedex.ContainsKey(pokemon.Id))
                return;

            // Cache the result per-guild so that geospatial stuff isn't queried for every single subscription below
            var locationCache = new Dictionary<ulong, Geofence>();
            Geofence GetGeofence(ulong guildId)
            {
                if (!locationCache.TryGetValue(guildId, out var geofence))
                {
                    var geofences = _config.Instance.Servers[guildId].Geofences;
                    geofence = GeofenceService.GetGeofence(geofences, new Coordinate(pokemon.Latitude, pokemon.Longitude));
                    locationCache.Add(guildId, geofence);
                }

                return geofence;
            }

            var subscriptions = await _subscriptionManager.GetSubscriptionsByPvpPokemonId(pokemon.Id);
            if (subscriptions == null)
            {
                _logger.LogWarning($"Failed to get subscriptions from database table.");
                return;
            }

            Subscription user;
            DiscordMember member = null;
            var pkmn = MasterFile.GetPokemon(pokemon.Id, pokemon.FormId);
            var matchesGreat = false;
            var matchesUltra = false;
            for (var i = 0; i < subscriptions.Count; i++)
            {
                //var start = DateTime.Now;
                try
                {
                    user = subscriptions[i];

                    if (!_config.Instance.Servers.ContainsKey(user.GuildId))
                        continue;

                    if (!_config.Instance.Servers[user.GuildId].Subscriptions.Enabled)
                        continue;

                    if (!_discordClients.ContainsKey(user.GuildId))
                        continue;

                    var client = _discordClients[user.GuildId];

                    try
                    {
                        member = await client.GetMemberById(user.GuildId, user.UserId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogDebug($"FAILED TO GET MEMBER BY ID {user.UserId}");
                        _logger.LogError($"Error: {ex}");
                        continue;
                    }

                    if (member?.Roles == null)
                        continue;

                    if (!member.HasSupporterRole(_config.Instance.Servers[user.GuildId].DonorRoleIds))
                    {
                        _logger.LogDebug($"User {member?.Username} ({user.UserId}) is not a supporter, skipping pvp pokemon {pkmn.Name}...");
                        // Automatically disable users subscriptions if not supporter to prevent issues
                        //user.Enabled = false;
                        //user.Save(false);
                        continue;
                    }

                    var form = Translator.Instance.GetFormName(pokemon.FormId);
                    var pokemonSubscriptions = user.PvP.Where(x =>
                        x.PokemonId == pokemon.Id &&
                        (string.IsNullOrEmpty(x.Form) || (!string.IsNullOrEmpty(x.Form) && string.Compare(x.Form, form, true) == 0))
                    );
                    foreach (var pkmnSub in pokemonSubscriptions)
                    {
                        matchesGreat = pokemon.GreatLeague != null && (pokemon.GreatLeague?.Exists(x => pkmnSub.League == PvpLeague.Great &&
                                                                         (x.CP ?? 0) >= Strings.MinimumGreatLeagueCP && (x.CP ?? 0) <= Strings.MaximumGreatLeagueCP &&
                                                                         (x.Rank ?? 4096) <= pkmnSub.MinimumRank &&
                                                                         (x.Percentage ?? 0) * 100 >= pkmnSub.MinimumPercent) ?? false);
                        matchesUltra = pokemon.UltraLeague != null && (pokemon.UltraLeague?.Exists(x => pkmnSub.League == PvpLeague.Ultra &&
                                                                         (x.CP ?? 0) >= Strings.MinimumUltraLeagueCP && (x.CP ?? 0) <= Strings.MaximumUltraLeagueCP &&
                                                                         (x.Rank ?? 4096) <= pkmnSub.MinimumRank &&
                                                                         (x.Percentage ?? 0) * 100 >= pkmnSub.MinimumPercent) ?? false);

                        // Check if Pokemon IV stats match any relevant great or ultra league ranks, if not skip.
                        if (!matchesGreat && !matchesUltra)
                            continue;

                        var geofence = GetGeofence(user.GuildId);
                        if (geofence == null)
                        {
                            //_logger.Warn($"Failed to lookup city from coordinates {pkmn.Latitude},{pkmn.Longitude} {db.Pokemon[pkmn.Id].Name} {pkmn.IV}, skipping...");
                            continue;
                        }

                        var globalLocation = user.Locations?.FirstOrDefault(x => string.Compare(x.Name, user.Location, true) == 0);
                        var subscriptionLocation = user.Locations?.FirstOrDefault(x => string.Compare(x.Name, pkmnSub.Location, true) == 0);
                        var globalDistanceMatches = globalLocation?.DistanceM > 0 && globalLocation?.DistanceM > new Coordinate(globalLocation?.Latitude ?? 0, globalLocation?.Longitude ?? 0).DistanceTo(new Coordinate(pokemon.Latitude, pokemon.Longitude));
                        var subscriptionDistanceMatches = subscriptionLocation?.DistanceM > 0 && subscriptionLocation?.DistanceM > new Coordinate(subscriptionLocation?.Latitude ?? 0, subscriptionLocation?.Longitude ?? 0).DistanceTo(new Coordinate(pokemon.Latitude, pokemon.Longitude));
                        var geofenceMatches = pkmnSub.Areas.Select(x => x.ToLower()).Contains(geofence.Name.ToLower());

                        // If set distance does not match and no geofences match, then skip Pokemon...
                        if (!globalDistanceMatches && !subscriptionDistanceMatches && !geofenceMatches)
                            continue;

                        var embed = pokemon.GenerateEmbedMessage(new AlarmMessageSettings
                        {
                            GuildId = user.GuildId,
                            Client = client,
                            Config = _config,
                            Alarm = null,
                            City = geofence.Name,
                        });
                        //var end = DateTime.Now.Subtract(start);
                        //_logger.Debug($"Took {end} to process PvP subscription for user {user.UserId}");
                        embed.Embeds.ForEach(x => _queue.Enqueue(new NotificationItem
                        {
                            Subscription = user,
                            Member = member,
                            Embed = x,
                            Description = pkmn.Name,
                            City = geofence.Name,
                        }));

                        // TODO: Statistics.Instance.SubscriptionPokemonSent++;
                        Thread.Sleep(5);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error: {ex}");
                }
            }

            subscriptions.Clear();
            subscriptions = null;
            member = null;
            user = null;
            pokemon = null;

            await Task.CompletedTask;
        }

        public async Task ProcessRaidSubscription(RaidData raid)
        {
            await Task.CompletedTask;
        }

        public async Task ProcessQuestSubscription(QuestData quest)
        {
            await Task.CompletedTask;
        }

        public async Task ProcessPokestopSubscription(PokestopData pokestop)
        {
            await Task.CompletedTask;
        }

        // TODO: Invasion
        
        // TODO: Lure

        // TODO: Gym
    }
}