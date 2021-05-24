using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.ReputationEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.ReputationService.Queries;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands.UserInfoCommands.UserReputationCommands.UserReputationListCommand
{
    [InjectableService]
    public class UserReputationListCommand : IUserReputationListCommand
    {
        private readonly IMediator _mediator;

        public UserReputationListCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем репутации пользователя
            var userReputations = await _mediator.Send(new GetUserReputationsQuery((long) context.User.Id));
            // получаем массив доступных репутаций
            var reputations = Enum.GetValues(typeof(Reputation)).Cast<Reputation>().ToArray();
            // получаем среднее значение репутаций пользователя
            var userAverageReputation =
                reputations.Sum(reputation =>
                    userReputations.ContainsKey(reputation) ? userReputations[reputation].Amount : 0) /
                reputations.Length;
            // определяем репутационный статус по среднему значению репутаций пользователя
            var userReputationStatus = ReputationStatusHelper.GetReputationStatus(userAverageReputation);

            var embed = new EmbedBuilder()
                // репутационный рейтинг и как смотреть информацию о репутации
                .WithDescription(
                    IzumiReplyMessage.UserReputationListDesc.Parse(
                        userReputationStatus.Localize(),
                        emotes.GetEmoteOrBlank(ReputationStatusHelper.Emote(userAverageReputation)),
                        userAverageReputation) +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // репутация пользователя
                .AddField(IzumiReplyMessage.UserReputationListFieldName.Parse(),
                    Enum.GetValues(typeof(Reputation))
                        .Cast<Reputation>()
                        .Aggregate(string.Empty, (current, reputation) =>
                            current +
                            $"{emotes.GetEmoteOrBlank("List")} `{reputation.GetHashCode()}` {emotes.GetEmoteOrBlank(reputation.Emote(userReputations.ContainsKey(reputation) ? userReputations[reputation].Amount : 0))} {(userReputations.ContainsKey(reputation) ? $"{userReputations[reputation].Amount}" : "0")} в **{reputation.Location().Localize(true)}**\n"))
                .WithFooter(IzumiReplyMessage.UserReputationListFooter.Parse());

            await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
            await Task.CompletedTask;
        }
    }
}
