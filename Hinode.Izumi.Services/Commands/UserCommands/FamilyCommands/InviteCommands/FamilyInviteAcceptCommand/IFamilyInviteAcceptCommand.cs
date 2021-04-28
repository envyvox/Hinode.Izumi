﻿using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.InviteCommands.FamilyInviteAcceptCommand
{
    public interface IFamilyInviteAcceptCommand
    {
        Task Execute(SocketCommandContext context, long inviteId);
    }
}