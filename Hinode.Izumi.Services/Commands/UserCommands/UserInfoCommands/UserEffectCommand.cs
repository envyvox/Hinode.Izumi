using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.EffectEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.EffectService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.TrainingService;
using Hinode.Izumi.Services.TimeService;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands
{
    [CommandCategory(CommandCategory.UserInfo)]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class UserEffectCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IEffectService _effectService;
        private readonly ITrainingService _trainingService;
        private readonly IImageService _imageService;
        private readonly ITimeService _timeService;

        public UserEffectCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IEffectService effectService, ITrainingService trainingService, IImageService imageService,
            ITimeService timeService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _effectService = effectService;
            _trainingService = trainingService;
            _imageService = imageService;
            _timeService = timeService;
        }

        [Command("эффекты"), Alias("effects")]
        [Summary("Посмотреть текущие эффекты")]
        public async Task UserEffectsTask()
        {
            // получаем все иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем эффекты пользователя
            var userEffects = await _effectService.GetUserEffect((long) Context.User.Id);
            // получаем категории эффектов
            var effectCategories = Enum.GetValues(typeof(EffectCategory))
                .Cast<EffectCategory>();

            var embed = new EmbedBuilder()
                // баннер эффектов
                .WithImageUrl(await _imageService.GetImageUrl(Image.Effects))
                .WithDescription(IzumiReplyMessage.UserEffectsDesc.Parse());

            // для каждой категории эффектов создаем embed field
            foreach (var category in effectCategories)
            {
                // получаем эффекты пользователя этой категории
                var userCategoryEffects = userEffects.Where(x => x.Category == category)
                    .ToArray();

                // если таких нет - переходим к следующей категории
                if (userCategoryEffects.Length < 1) continue;

                embed.AddField(
                    // название категории
                    $"{emotes.GetEmoteOrBlank("List")} {category.Localize()}",
                    // выводим список эффектов пользователя в этой категории
                    userCategoryEffects.Aggregate(string.Empty, (current, effect) =>
                        current +
                        $"{emotes.GetEmoteOrBlank("CardDeck")} {effect.Effect.Localize()}" +
                        (effect.Expiration != null
                            // если эффект имеет время действия, выводим сколько осталось времени до окончания
                            ? _timeService.TimeLeft((DateTimeOffset) effect.Expiration)
                            : "") + "\n"));
            }

            // если у пользователя меньше 2 эффектов, добавляем embed field с подсказкой как получить эффекты
            if (embed.Fields.Count < 2)
                embed.AddField(IzumiReplyMessage.UserEffectsHelpFieldName.Parse(emotes.GetEmoteOrBlank("List")),
                    IzumiReplyMessage.UserEffectsHelpFieldDesc.Parse(emotes.GetEmoteOrBlank("CardDeck")));

            await _discordEmbedService.SendEmbed(Context.User, embed);
            // проверяем нужно ли двинуть прогресс обучения пользователя
            await _trainingService.CheckStep((long) Context.User.Id, TrainingStep.CheckEffects);
            await Task.CompletedTask;
        }
    }
}
