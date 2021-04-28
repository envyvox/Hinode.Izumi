using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.Commands.UserCommands.MarketCommands.MarketBuyCommands.MarketCheckTopSellingRequestsCommand;
using Hinode.Izumi.Services.Commands.UserCommands.MarketCommands.MarketBuyCommands.MarketCreateBuyRequestCommand;
using Hinode.Izumi.Services.Commands.UserCommands.MarketCommands.MarketBuyCommands.MarketDirectBuyCommand;
using Hinode.Izumi.Services.Commands.UserCommands.MarketCommands.MarketInfoCommand;
using Hinode.Izumi.Services.Commands.UserCommands.MarketCommands.MarketRequestCommands.MarketRequestCancelCommand;
using Hinode.Izumi.Services.Commands.UserCommands.MarketCommands.MarketRequestCommands.MarketRequestListCommand;
using Hinode.Izumi.Services.Commands.UserCommands.MarketCommands.MarketSellCommands.MarketCheckTopBuyingRequestsCommand;
using Hinode.Izumi.Services.Commands.UserCommands.MarketCommands.MarketSellCommands.MarketCreateSellRequestCommand;
using Hinode.Izumi.Services.Commands.UserCommands.MarketCommands.MarketSellCommands.MarketDirectSellCommand;

namespace Hinode.Izumi.Services.Commands.UserCommands.MarketCommands
{
    [Group("рынок"), Alias("group")]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    [IzumiRequireLocation(Location.CapitalMarket), IzumiRequireNoDebuff(BossDebuff.CapitalStop)]
    public class MarketCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IMarketInfoCommand _marketInfoCommand;

        public MarketCommands(IMarketInfoCommand marketInfoCommand)
        {
            _marketInfoCommand = marketInfoCommand;
        }

        [Command]
        public async Task MarketInfoTask() =>
            await _marketInfoCommand.Execute(Context);

        [Group("купить"), Alias("buy")]
        public class MarketBuyCommands : ModuleBase<SocketCommandContext>
        {
            private readonly IMarketCheckTopSellingRequestsCommand _marketCheckTopSellingRequestsCommand;
            private readonly IMarketDirectBuyCommand _marketDirectBuyCommand;
            private readonly IMarketCreateBuyRequestCommand _marketCreateBuyRequestCommand;

            public MarketBuyCommands(IMarketCheckTopSellingRequestsCommand marketCheckTopSellingRequestsCommand,
                IMarketDirectBuyCommand marketDirectBuyCommand,
                IMarketCreateBuyRequestCommand marketCreateBuyRequestCommand)
            {
                _marketCheckTopSellingRequestsCommand = marketCheckTopSellingRequestsCommand;
                _marketDirectBuyCommand = marketDirectBuyCommand;
                _marketCreateBuyRequestCommand = marketCreateBuyRequestCommand;
            }

            [Command]
            public async Task MarketCheckTopSellingRequestsTask(MarketCategory category,
                [Remainder] string pattern = null) =>
                await _marketCheckTopSellingRequestsCommand.Execute(Context, category, pattern);

            [Command]
            public async Task MarketDirectBuyTask(long requestId, long amount = 1) =>
                await _marketDirectBuyCommand.Execute(Context, requestId, amount);

            [Command]
            public async Task MarketCreateBuyRequestTask(MarketCategory category, string pattern, long price,
                long amount = 1) =>
                await _marketCreateBuyRequestCommand.Execute(Context, category, pattern, price, amount);
        }

        [Group("продать"), Alias("sell")]
        public class MarketSellCommands : ModuleBase<SocketCommandContext>
        {
            private readonly IMarketCheckTopBuyingRequestsCommand _marketCheckTopBuyingRequestsCommand;
            private readonly IMarketDirectSellCommand _marketDirectSellCommand;
            private readonly IMarketCreateSellRequestCommand _marketCreateSellRequestCommand;

            public MarketSellCommands(IMarketCheckTopBuyingRequestsCommand marketCheckTopBuyingRequestsCommand,
                IMarketDirectSellCommand marketDirectSellCommand,
                IMarketCreateSellRequestCommand marketCreateSellRequestCommand)
            {
                _marketCheckTopBuyingRequestsCommand = marketCheckTopBuyingRequestsCommand;
                _marketDirectSellCommand = marketDirectSellCommand;
                _marketCreateSellRequestCommand = marketCreateSellRequestCommand;
            }

            [Command]
            public async Task MarketCheckTopBuyingRequestsTask(MarketCategory category,
                [Remainder] string pattern = null) =>
                await _marketCheckTopBuyingRequestsCommand.Execute(Context, category, pattern);

            [Command]
            public async Task MarketDirectSellTask(long requestId, long amount = 1) =>
                await _marketDirectSellCommand.Execute(Context, requestId, amount);

            [Command]
            public async Task MarketCreateSellRequestTask(MarketCategory category, string pattern, long price,
                long amount = 1) =>
                await _marketCreateSellRequestCommand.Execute(Context, category, pattern, price, amount);
        }

        [Group("заявки"), Alias("requests")]
        public class MarketRequestCommands : ModuleBase<SocketCommandContext>
        {
            private readonly IMarketRequestListCommand _marketRequestListCommand;
            private readonly IMarketRequestCancelCommand _marketRequestCancelCommand;

            public MarketRequestCommands(IMarketRequestListCommand marketRequestListCommand,
                IMarketRequestCancelCommand marketRequestCancelCommand)
            {
                _marketRequestListCommand = marketRequestListCommand;
                _marketRequestCancelCommand = marketRequestCancelCommand;
            }

            [Command]
            public async Task MarketRequestListTask() =>
                await _marketRequestListCommand.Execute(Context);

            [Command("отменить"), Alias("cancel")]
            public async Task MarketRequestCancelTask(long requestId) =>
                await _marketRequestCancelCommand.Execute(Context, requestId);
        }
    }
}
