using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Commands.Attributes;
using Hinode.Izumi.Commands.UserCommands.ShopCommands.FisherCommands.FisherListCommand;
using Hinode.Izumi.Commands.UserCommands.ShopCommands.FisherCommands.FisherSellCommand;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;

namespace Hinode.Izumi.Commands.UserCommands.ShopCommands.FisherCommands
{
    [CommandCategory(CommandCategory.Shop)]
    [Group("рыбак"), Alias("fisher")]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    [IzumiRequireLocation(Location.Seaport), IzumiRequireNoDebuff(BossDebuff.SeaportStop)]
    public class FisherCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IFisherListCommand _fisherListCommand;
        private readonly IFisherSellCommand _fisherSellCommand;

        public FisherCommands(IFisherListCommand fisherListCommand, IFisherSellCommand fisherSellCommand)
        {
            _fisherListCommand = fisherListCommand;
            _fisherSellCommand = fisherSellCommand;
        }

        [Command("")]
        public async Task FisherListTask() =>
            await _fisherListCommand.Execute(Context);

        [Command("продать"), Alias("sell")]
        // этот метод вызывается когда пользователь хочет продать определеную рыбу с указанием количества
        public async Task FisherSellTask(long fishId, long amount = 1) =>
            await _fisherSellCommand.SellFishWithIdAndAmount(Context, fishId, amount);

        [Command("продать"), Alias("sell")]
        // этот метод вызывается когда пользователь хочет продать всю определеную рыбу
        public async Task FisherSellTask(long fishId, string input) =>
            await _fisherSellCommand.SellAllFishWithId(Context, fishId, input);

        [Command("продать"), Alias("sell")]
        // этот метод вызывается когда пользователь хочет продать всю имеющуюся у него рыбу
        public async Task FisherSellTask(string input) =>
            await _fisherSellCommand.SellAllFish(Context, input);
    }
}
