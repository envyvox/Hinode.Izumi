using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums.AchievementEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserAchievementsCommands.UserAchievementsCommand
{
    [InjectableService]
    public class UserAchievementsCommand : IUserAchievementsCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IImageService _imageService;

        public UserAchievementsCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IImageService imageService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _imageService = imageService;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем все иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем все категории достижений
            var categories = Enum.GetValues(typeof(AchievementCategory)).Cast<AchievementCategory>();

            var embed = new EmbedBuilder()
                // баннер достижений
                .WithImageUrl(await _imageService.GetImageUrl(Image.Achievements))
                // рассказываем как просматривать достижения по категориям
                .WithDescription(IzumiReplyMessage.AchievementGroupsDesc.Parse())
                // выводим список доступных категорий
                .AddField(IzumiReplyMessage.AchievementGroupsFieldName.Parse(),
                    categories
                        .Aggregate(string.Empty, (current, category) =>
                            current +
                            $"{emotes.GetEmoteOrBlank("List")} `{category.GetHashCode()}` {category.Localize()}\n"));

            await _discordEmbedService.SendEmbed(context.User, embed);
            await Task.CompletedTask;
        }
    }
}
