﻿using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.MarketCommands.MarketBuyCommands.MarketDirectBuyCommand
{
    public interface IMarketDirectBuyCommand
    {
        Task Execute(SocketCommandContext context, long requestId, long amount = 1);
    }
}
