﻿using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.CurrencyCommands.FamilyCurrencyAddCommand
{
    public interface IFamilyCurrencyAddCommand
    {
        Task Execute(SocketCommandContext context, long amount, string currencyNamePattern);
    }
}