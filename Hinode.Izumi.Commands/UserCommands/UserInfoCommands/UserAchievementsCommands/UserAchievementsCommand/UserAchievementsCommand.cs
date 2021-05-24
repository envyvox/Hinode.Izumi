using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums.AchievementEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.UserInfoCommands.UserAchievementsCommands.UserAchievementsCommand
{
    [InjectableService]
    public class UserAchievementsCommand : IUserAchievementsCommand
    {
        private readonly IMediator _mediator;

        public UserAchievementsCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем все иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем все категории достижений
            var categories = Enum.GetValues(typeof(AchievementCategory)).Cast<AchievementCategory>();

            var embed = new EmbedBuilder()
                // баннер достижений
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.Achievements)))
                // рассказываем как просматривать достижения по категориям
                .WithDescription(IzumiReplyMessage.AchievementGroupsDesc.Parse())
                // выводим список доступных категорий
                .AddField(IzumiReplyMessage.AchievementGroupsFieldName.Parse(),
                    categories
                        .Aggregate(string.Empty, (current, category) =>
                            current +
                            $"{emotes.GetEmoteOrBlank("List")} `{category.GetHashCode()}` {category.Localize()}\n"));

            await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
            await Task.CompletedTask;
        }
    }
}
