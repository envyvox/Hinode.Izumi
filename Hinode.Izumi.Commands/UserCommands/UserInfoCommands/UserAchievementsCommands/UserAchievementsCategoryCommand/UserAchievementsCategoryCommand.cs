using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.AchievementEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.AchievementService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.UserInfoCommands.UserAchievementsCommands.
    UserAchievementsCategoryCommand
{
    [InjectableService]
    public class UserAchievementsCategoryCommand : IUserAchievementsCategoryCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public UserAchievementsCategoryCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context, AchievementCategory category)
        {
            // получаем все иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем все достижения пользователя в этой категории
            var userAchievements = await _mediator.Send(new GetUserAchievementsByCategoryQuery(
                (long) context.User.Id, category));

            var embed = new EmbedBuilder()
                // название категории
                .WithAuthor(category.Localize())
                // баннер достижений
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.Achievements)));

            // для каждого достижения в этой категории создаем embed field
            foreach (var achievement in category.Achievements())
            {
                // получаем информацию о достижении из базы
                var achievementRecord = await _mediator.Send(new GetAchievementByTypeQuery(achievement));
                // получаем это достижение у пользователя
                var userAchievement = userAchievements.FirstOrDefault(x =>
                    x.AchievementId == achievementRecord.Id);
                // определяем какую иконку нужно отобразить в зависимости от того, выполнил ли пользователь достижение
                var achievementEmote = emotes.GetEmoteOrBlank(
                    "Achievement" + (userAchievement is null ? "BW" : ""));
                // определяем какая награда за выполнение достижения
                var achievementReward = achievementRecord.Reward switch
                {
                    AchievementReward.Ien =>
                        $"{emotes.GetEmoteOrBlank(Currency.Ien.ToString())} {achievementRecord.Number} {_local.Localize(Currency.Ien.ToString(), achievementRecord.Number)}",

                    AchievementReward.Title =>
                        $"титул {emotes.GetEmoteOrBlank(((Title) achievementRecord.Number).Emote())} {((Title) achievementRecord.Number).Localize()}",

                    AchievementReward.Pearl =>
                        $"{emotes.GetEmoteOrBlank(Currency.Pearl.ToString())} {achievementRecord.Number} {_local.Localize(Currency.Pearl.ToString(), achievementRecord.Number)}",

                    _ => throw new ArgumentOutOfRangeException()
                };

                embed.AddField(
                    // выводим иконку и название достижения
                    $"{achievementEmote} {achievementRecord.Type.Localize()}",
                    // выводим награду за достижение
                    $"Награда: {achievementReward}" +
                    // если пользователь выполнил достижение, выводим время выполнения
                    (userAchievement is null
                        ? ""
                        : $"\nВыполнено в {userAchievement.CreatedAt.ToString("HH:MM, dd MMMM yyyy", new CultureInfo("ru-ru"))}"
                    ));
            }

            await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
            await Task.CompletedTask;
        }
    }
}
