using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.AchievementEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.AchievementService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserAchievementsCommands.
    UserAchievementsCategoryCommand
{
    [InjectableService]
    public class UserAchievementsCategoryCommand : IUserAchievementsCategoryCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IImageService _imageService;
        private readonly IAchievementService _achievementService;
        private readonly ILocalizationService _local;

        public UserAchievementsCategoryCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IImageService imageService, IAchievementService achievementService, ILocalizationService local)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _imageService = imageService;
            _achievementService = achievementService;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context, AchievementCategory category)
        {
            // получаем все иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем все достижения пользователя в этой категории
            var userAchievements = await _achievementService.GetUserAchievement(
                (long) context.User.Id, category);

            var embed = new EmbedBuilder()
                // название категории
                .WithAuthor(category.Localize())
                // баннер достижений
                .WithImageUrl(await _imageService.GetImageUrl(Image.Achievements));

            // для каждого достижения в этой категории создаем embed field
            foreach (var achievement in category.Achievements())
            {
                // получаем информацию о достижении из базы
                var achievementModel = await _achievementService.GetAchievement(achievement.GetHashCode());
                // получаем это достижение у пользователя
                var userAchievement = userAchievements.FirstOrDefault(x =>
                    x.AchievementId == achievement.GetHashCode());
                // определяем какую иконку нужно отобразить в зависимости от того, выполнил ли пользователь достижение
                var achievementEmote = emotes.GetEmoteOrBlank(
                    "Achievement" + (userAchievement == null ? "BW" : ""));
                // определяем какая награда за выполнение достижения
                var achievementReward = achievementModel.Reward switch
                {
                    AchievementReward.Ien =>
                        $"{emotes.GetEmoteOrBlank(Currency.Ien.ToString())} {achievementModel.Number} {_local.Localize(Currency.Ien.ToString(), achievementModel.Number)}",

                    AchievementReward.Title =>
                        $"титул {emotes.GetEmoteOrBlank(((Title) achievementModel.Number).Emote())} {((Title) achievementModel.Number).Localize()}",

                    AchievementReward.Pearl =>
                        $"{emotes.GetEmoteOrBlank(Currency.Pearl.ToString())} {achievementModel.Number} {_local.Localize(Currency.Pearl.ToString(), achievementModel.Number)}",

                    _ => throw new ArgumentOutOfRangeException()
                };

                embed.AddField(
                    // выводим иконку и название достижения
                    $"{achievementEmote} {achievementModel.Type.Localize()}",
                    // выводим награду за достижение
                    $"Награда: {achievementReward}" +
                    // если пользователь выполнил достижение, выводим время выполнения
                    (userAchievement == null
                        ? ""
                        : $"\nВыполнено в {userAchievement.CreatedAt.ToString("HH:MM, dd MMMM yyyy", new CultureInfo("ru-ru"))}"
                    ));
            }

            await _discordEmbedService.SendEmbed(context.User, embed);
            await Task.CompletedTask;
        }
    }
}
