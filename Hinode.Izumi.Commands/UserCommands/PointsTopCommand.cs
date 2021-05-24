using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Commands.Attributes;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.UserService.Queries;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands
{
    [CommandCategory(CommandCategory.Rating)]
    [IzumiRequireRegistry]
    public class PointsTopCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public PointsTopCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        [Command("топ"), Alias("рейтинг", "top", "rating")]
        [Summary("Посмотреть рейтинг приключений")]
        public async Task PointsTopTask()
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем топ-10 пользователей
            var users = await _mediator.Send(new GetTopUsersQuery());
            // получаем пользователя
            var user = await _mediator.Send(new GetUserWithRowNumberByIdQuery((long) Context.User.Id));
            // получаем отображение позиции пользователя
            var userRowNumber = await _mediator.Send(new DisplayRowNumberQuery(user.RowNumber));

            var usersInTopString = string.Empty;
            foreach (var topUser in users)
            {
                // получаем отображение позиции пользователя
                var topUserRowNumber = await _mediator.Send(new DisplayRowNumberQuery(topUser.RowNumber));
                // заполняем локализированную строку
                usersInTopString +=
                    $"{topUserRowNumber} {emotes.GetEmoteOrBlank(topUser.Title.Emote())} {topUser.Title.Localize()} **{topUser.Name}** {topUser.Points} {_local.Localize("AdventurePoints", topUser.Points)}\n";
            }

            var embed = new EmbedBuilder()
                // позиция пользователя
                .WithDescription(
                    IzumiReplyMessage.PointsTopDesc.Parse(
                        userRowNumber, user.RowNumber, user.Points, _local.Localize("AdventurePoints", user.Points)) +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // топ пользователей
                .AddField(IzumiReplyMessage.PointsTopFieldName.Parse(), usersInTopString)
                // рассказываем про сброс очков приключений
                .WithFooter(IzumiReplyMessage.PointsTopFooter.Parse());

            await _mediator.Send(new SendEmbedToUserCommand(Context.User, embed));
            await Task.CompletedTask;
        }
    }
}
