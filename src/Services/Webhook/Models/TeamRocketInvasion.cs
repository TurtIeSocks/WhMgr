﻿namespace WhMgr.Services.Webhook.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json.Serialization;

    using WhMgr.Data;

    public class TeamRocketInvasion
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("grunt")]
        public string Grunt { get; set; }

        [JsonPropertyName("second_reward")]
        public string SecondReward { get; set; }

        [JsonPropertyName("encounters")]
        public TeamRocketEncounters Encounters { get; set; }

        [JsonIgnore]
        public bool HasEncounter => Encounters?.First?.Count > 0 || Encounters?.Second?.Count > 0 || Encounters?.Third?.Count > 0;

        public TeamRocketInvasion()
        {
            Encounters = new TeamRocketEncounters();
        }

        public string GetPossibleInvasionEncounters()
        {
            var toInt = new Func<string, uint>(x =>
            {
                var val = x.Split('_')[0];
                if (!uint.TryParse(val, out var result))
                {
                    Console.Error.WriteLine($"Failed to parse {val} as integer");
                }
                return result;
            });
            var first = string.Join(", ", Encounters.First.Select(x => MasterFile.GetPokemon(toInt(x), 0)?.Name));
            var second = string.Join(", ", Encounters.Second.Select(x => MasterFile.GetPokemon(toInt(x), 0)?.Name));
            //var third = string.Join(", ", invasion.Encounters.Third.Select(x => Database.Instance.Pokemon[x].Name));
            var msg = string.Empty;
            if (SecondReward == "true")
            {
                //85%/15% Rate
                msg += $"85% - {first}\r\n";
                msg += $"15% - {second}\r\n";
            }
            else
            {
                //100% Rate
                msg += $"100% - {first}\r\n";
            }
            return msg;
        }

        public List<int> GetEncounterRewards()
        {
            var list = new List<int>();
            if (Encounters == null)
                return list;

            if (SecondReward == "true")
            {
                //85%/15% Rate
                for (var i = 0; i < Encounters.Second.Count; i++)
                {
                    var mon = Encounters.Second[i];
                    var id = ParsePokemonId(mon);
                    if (id == 0)
                        continue;

                    list.Add(id);
                }
            }
            else
            {
                //100% Rate
                for (var i = 0; i < Encounters.First.Count; i++)
                {
                    var mon = Encounters.First[i];
                    var id = ParsePokemonId(mon);
                    if (id == 0)
                        continue;

                    list.Add(id);
                }
            }
            return list;
        }

        private static int ParsePokemonId(string value)
        {
            var split = value.Split('_');
            if (!int.TryParse(split[0], out var id))
            {
                Console.WriteLine($"Failed to parse grunttype {split[0]}");
                return 0;
            }
            return id;
        }
    }

    public class TeamRocketEncounters
    {
        [JsonPropertyName("first")]
        public List<string> First { get; set; }

        [JsonPropertyName("second")]
        public List<string> Second { get; set; }

        [JsonPropertyName("third")]
        public List<string> Third { get; set; }

        public TeamRocketEncounters()
        {
            First = new List<string>();
            Second = new List<string>();
            Third = new List<string>();
        }
    }
}