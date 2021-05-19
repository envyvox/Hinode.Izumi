using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.CalculationService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.UserService;

namespace Hinode.Izumi.Services.Commands.UserCommands
{
    [CommandCategory(CommandCategory.Rating)]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class PointsTopCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IUserService _userService;
        private readonly ICalculationService _calc;
        private readonly ILocalizationService _local;

        public PointsTopCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IUserService userService, ICalculationService calc, ILocalizationService local)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _userService = userService;
            _calc = calc;
            _local = local;
        }

        [Command("топ"), Alias("рейтинг", "top", "rating")]
        [Summary("Посмотреть рейтинг приключений")]
        public async Task PointsTopTask()
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем массив пользователей
            var users = await _userService.GetTopUsers();
            // получаем пользователя
            var user = await _userService.GetUserWithRowNumber((long) Context.User.Id);

            var embed = new EmbedBuilder()
                // позиция пользователя
                .WithDescription(
                    IzumiReplyMessage.PointsTopDesc.Parse(
                        _calc.RowNumberEmote(emotes, user.RowNumber), user.RowNumber, user.Points,
                        _local.Localize("AdventurePoints", user.Points)) +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // топ пользователей
                .AddField(IzumiReplyMessage.PointsTopFieldName.Parse(),
                    users.Aggregate(string.Empty, (current, userWr) =>
                        current +
                        $"{_calc.RowNumberEmote(emotes, userWr.RowNumber)} {emotes.GetEmoteOrBlank(userWr.Title.Emote())} {userWr.Title.Localize()} **{userWr.Name}** {userWr.Points} {_local.Localize("AdventurePoints", userWr.Points)}\n"))
                // рассказываем про сброс очков приключений
                .WithFooter(IzumiReplyMessage.PointsTopFooter.Parse());

            await _discordEmbedService.SendEmbed(Context.User, embed);
            await Task.CompletedTask;
        }
    }
}
