﻿namespace WhMgr.Services.Discord
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DSharpPlus;

    public interface IDiscordClientService
    {
        IReadOnlyDictionary<ulong, DiscordClient> DiscordClients { get; }

        Task Start();

        Task Stop();
    }
}