﻿using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.ContractCommands.ContractListCommand
{
    public interface IContractListCommand
    {
        Task Execute(SocketCommandContext context);
    }
}