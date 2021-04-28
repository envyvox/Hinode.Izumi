﻿using System.Threading.Tasks;
using Discord.WebSocket;

namespace Hinode.Izumi.Services.DiscordServices.DiscordClientService.ClientOnServices.UserVoiceStateUpdated
{
    public interface IUserVoiceStateUpdatedService
    {
        Task Execute(SocketUser user, SocketVoiceState userOldState, SocketVoiceState userNewState);
    }
}
