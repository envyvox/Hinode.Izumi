using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Commands.UserCommands.MarketCommands.MarketBuyCommands.MarketCheckTopSellingRequestsCommand;
using Hinode.Izumi.Commands.UserCommands.MarketCommands.MarketBuyCommands.MarketCreateBuyRequestCommand;
using Hinode.Izumi.Commands.UserCommands.MarketCommands.MarketBuyCommands.MarketDirectBuyCommand;
using Hinode.Izumi.Commands.UserCommands.MarketCommands.MarketInfoCommand;
using Hinode.Izumi.Commands.UserCommands.MarketCommands.MarketRequestCommands.MarketRequestCancelCommand;
using Hinode.Izumi.Commands.UserCommands.MarketCommands.MarketRequestCommands.MarketRequestListCommand;
using Hinode.Izumi.Commands.UserCommands.MarketCommands.MarketSellCommands.MarketCheckTopBuyingRequestsCommand;
using Hinode.Izumi.Commands.UserCommands.MarketCommands.MarketSellCommands.MarketCreateSellRequestCommand;
using Hinode.Izumi.Commands.UserCommands.MarketCommands.MarketSellCommands.MarketDirectSellCommand;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.WebServices.CommandWebService.Attributes;

namespace Hinode.Izumi.Commands.UserCommands.MarketCommands
{
    [CommandCategory(CommandCategory.Market)]
    [Group("рынок"), Alias("group")]
    [IzumiRequireRegistry]
    [IzumiRequireLocation(Location.CapitalMarket), IzumiRequireNoDebuff(BossDebuff.CapitalStop)]
    public class MarketCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IMarketInfoCommand _marketInfoCommand;

        public MarketCommands(IMarketInfoCommand marketInfoCommand)
        {
            _marketInfoCommand = marketInfoCommand;
        }

        [Command("")]
        [Summary("Посмотреть информацию о командах рынка")]
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

            [Command("")]
            [Summary("Посмотреть лучшие заявки в указанной категори или даже по указанному предмету")]
            [CommandUsage("!рынок купить 1", "!рынок купить 1 уголь")]
            public async Task MarketCheckTopSellingRequestsTask(
                [Summary("Номер категории")] MarketCategory category,
                [Summary("Название предмета")] [Remainder] string pattern = null) =>
                await _marketCheckTopSellingRequestsCommand.Execute(Context, category, pattern);

            [Command("")]
            [Summary("Купить предметы по указанной заявке")]
            [CommandUsage("!рынок купить 323 10", "!рынок купить 234")]
            public async Task MarketDirectBuyTask(
                [Summary("Номер заявки")] long requestId,
                [Summary("Количество")] long amount = 1) =>
                await _marketDirectBuyCommand.Execute(Context, requestId, amount);

            [Command("")]
            [Summary("Создать заявку на покупку указанного предмета")]
            [CommandUsage("!рынок купить 1 уголь 5 10", "!рынок купить 3 вино 600")]
            public async Task MarketCreateBuyRequestTask(
                [Summary("Номер категории")] MarketCategory category,
                [Summary("Название предмета")] string pattern,
                [Summary("Цена за единицу")] long price,
                [Summary("Количество")] long amount = 1) =>
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

            [Command("")]
            [Summary("Посмотреть лучшие заявки в указанной категори или даже по указанному предмету")]
            [CommandUsage("!рынок продать  1", "!рынок продать 1 уголь")]
            public async Task MarketCheckTopBuyingRequestsTask(
                [Summary("Номер категории")] MarketCategory category,
                [Summary("Название предмета")] [Remainder] string pattern = null) =>
                await _marketCheckTopBuyingRequestsCommand.Execute(Context, category, pattern);

            [Command("")]
            [Summary("Продать предметы по указанной заявке")]
            [CommandUsage("!рынок продать 323 10", "!рынок продать 234")]
            public async Task MarketDirectSellTask(
                [Summary("Номер заявки")] long requestId,
                [Summary("Количество")] long amount = 1) =>
                await _marketDirectSellCommand.Execute(Context, requestId, amount);

            [Command("")]
            [Summary("Создать заявку на продажу указанного предмета")]
            [CommandUsage("!рынок продать 1 уголь 5 10", "!рынок продать 3 вино 600")]
            public async Task MarketCreateSellRequestTask(
                [Summary("Номер категории")] MarketCategory category,
                [Summary("Название предмета")] string pattern,
                [Summary("Цена за единицу")] long price,
                [Summary("Количество")] long amount = 1) =>
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

            [Command("")]
            [Summary("Посмотреть созданные вами заявки на рынке")]
            public async Task MarketRequestListTask() =>
                await _marketRequestListCommand.Execute(Context);

            [Command("отменить"), Alias("cancel")]
            [Summary("Отменить созданную вами заявку на рынке")]
            [CommandUsage("!рынок заявки отменить 324")]
            public async Task MarketRequestCancelTask(
                [Summary("Номер заявки")] long requestId) =>
                await _marketRequestCancelCommand.Execute(Context, requestId);
        }
    }
}
