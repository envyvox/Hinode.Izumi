using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Commands.Attributes;
using Hinode.Izumi.Commands.UserCommands.CasinoCommands.LotteryCommands.LotteryBuyCommand;
using Hinode.Izumi.Commands.UserCommands.CasinoCommands.LotteryCommands.LotteryGiftCommand;
using Hinode.Izumi.Commands.UserCommands.CasinoCommands.LotteryCommands.LotteryInfoCommand;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;

namespace Hinode.Izumi.Commands.UserCommands.CasinoCommands.LotteryCommands
{
    [CommandCategory(CommandCategory.Casino)]
    [Group("лотерея"), Alias("lottery")]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    [IzumiRequireLocation(Location.CapitalCasino), IzumiRequireNoDebuff(BossDebuff.CapitalStop)]
    [IzumiRequireCasinoOpen]
    public class LotteryCommands : ModuleBase<SocketCommandContext>
    {
        private readonly ILotteryInfoCommand _lotteryInfoCommand;
        private readonly ILotteryBuyCommand _lotteryBuyCommand;
        private readonly ILotteryGiftCommand _lotteryGiftCommand;

        public LotteryCommands(ILotteryInfoCommand lotteryInfoCommand, ILotteryBuyCommand lotteryBuyCommand,
            ILotteryGiftCommand lotteryGiftCommand)
        {
            _lotteryInfoCommand = lotteryInfoCommand;
            _lotteryBuyCommand = lotteryBuyCommand;
            _lotteryGiftCommand = lotteryGiftCommand;
        }

        [Command("")]
        [Summary("Посмотреть информацию о лотерее")]
        public async Task LotteryInfoTask() =>
            await _lotteryInfoCommand.Execute(Context);

        [Command("купить"), Alias("buy")]
        [Summary("Приобрести лотерейный билет")]
        public async Task LotteryBuyTask() =>
            await _lotteryBuyCommand.Execute(Context);

        [Command("подарить"), Alias("gift")]
        [Summary("Подарить лотереный билет указанному пользователю")]
        public async Task LotteryGiftTask(
            [Summary("Игровое имя")] [Remainder] string namePattern) =>
            await _lotteryGiftCommand.Execute(Context, namePattern);
    }
}
