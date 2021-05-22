using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Commands.Attributes;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.EffectEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.EffectService.Queries;
using Hinode.Izumi.Services.GameServices.TutorialService.Commands;
using Hinode.Izumi.Services.ImageService.Queries;
using Hinode.Izumi.Services.TimeService;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.UserInfoCommands
{
    [CommandCategory(CommandCategory.UserInfo)]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class UserEffectCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;
        private readonly ITimeService _timeService;

        public UserEffectCommand(IMediator mediator, ITimeService timeService)
        {
            _mediator = mediator;
            _timeService = timeService;
        }

        [Command("эффекты"), Alias("effects")]
        [Summary("Посмотреть текущие эффекты")]
        public async Task UserEffectsTask()
        {
            // получаем все иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем эффекты пользователя
            var userEffects = await _mediator.Send(new GetUserEffectsQuery((long) Context.User.Id));
            // получаем категории эффектов
            var effectCategories = Enum.GetValues(typeof(EffectCategory))
                .Cast<EffectCategory>();

            var embed = new EmbedBuilder()
                // баннер эффектов
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.Effects)))
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
                        (effect.Expiration is not null
                            // если эффект имеет время действия, выводим сколько осталось времени до окончания
                            ? _timeService.TimeLeft((DateTimeOffset) effect.Expiration)
                            : "") + "\n"));
            }

            // если у пользователя меньше 2 эффектов, добавляем embed field с подсказкой как получить эффекты
            if (embed.Fields.Count < 2)
                embed.AddField(IzumiReplyMessage.UserEffectsHelpFieldName.Parse(emotes.GetEmoteOrBlank("List")),
                    IzumiReplyMessage.UserEffectsHelpFieldDesc.Parse(emotes.GetEmoteOrBlank("CardDeck")));

            await _mediator.Send(new SendEmbedToUserCommand(Context.User, embed));
            // проверяем нужно ли двинуть прогресс обучения пользователя
            await _mediator.Send(new CheckUserTutorialStepCommand((long) Context.User.Id, TutorialStep.CheckEffects));
            await Task.CompletedTask;
        }
    }
}
