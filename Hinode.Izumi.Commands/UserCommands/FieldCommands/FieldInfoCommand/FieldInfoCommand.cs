using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.EmoteService.Records;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.CropService.Queries;
using Hinode.Izumi.Services.GameServices.FieldService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using Hinode.Izumi.Services.GameServices.SeedService.Queries;
using Hinode.Izumi.Services.GameServices.TutorialService.Commands;
using Hinode.Izumi.Services.ImageService.Queries;
using Humanizer;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.FieldCommands.FieldInfoCommand
{
    [InjectableService]
    public class FieldInfoCommand : IFieldInfoCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;
        private Dictionary<string, EmoteRecord> _emotes;

        public FieldInfoCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            _emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем участок пользователя
            var userFields = await _mediator.Send(new GetUserFieldsQuery((long) context.User.Id));

            var embed = new EmbedBuilder()
                // баннер участка
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.Field)));

            // если у пользователя нет клеток участка, предлагаем ему купить участок
            if (userFields.Length < 1)
            {
                // получаем стоимость участка земли
                var fieldPrice = await _mediator.Send(new GetPropertyValueQuery(Property.FieldPrice));
                // выводим предложение о покупке участка
                embed
                    .WithDescription(IzumiReplyMessage.FieldInfoNullDesc.Parse())
                    .AddField(IzumiReplyMessage.FieldInfoNullFieldName.Parse(),
                        IzumiReplyMessage.FieldInfoNullFieldDesc.Parse(
                            _emotes.GetEmoteOrBlank(Currency.Ien.ToString()), fieldPrice,
                            _local.Localize(Currency.Ien.ToString(), fieldPrice)));
            }
            else
            {
                // показываем команды участка
                embed.AddField(IzumiReplyMessage.FieldInfoHarvestingFieldName.Parse(),
                    IzumiReplyMessage.FieldInfoHarvestingFieldDesc.Parse() +
                    $"\n{_emotes.GetEmoteOrBlank("Blank")}");

                // создаем embed field для каждой клетки земли
                foreach (var field in userFields)
                {
                    string fieldNameString;
                    string fieldDescString;

                    // получаем семена посаженные на этой клетке земли
                    var seed = await _mediator.Send(new GetSeedQuery(field.SeedId));

                    // заполняем информацию о клетке земли в зависимости от ее состояния
                    switch (field.State)
                    {
                        case FieldState.Empty:

                            fieldNameString = IzumiReplyMessage.FieldEmptyFieldName.Parse();
                            fieldDescString = IzumiReplyMessage.FieldEmptyFieldDesc.Parse();

                            break;
                        case FieldState.Planted:

                            fieldNameString =
                                $"{_emotes.GetEmoteOrBlank(seed.Name)} {_local.Localize(seed.Name)}, " +
                                IzumiReplyMessage.FieldProgress.Parse(
                                    field.ReGrowth
                                        ? (seed.ReGrowth - field.Progress).Days().Humanize(1, new CultureInfo("ru-RU"))
                                        : (seed.Growth - field.Progress).Days().Humanize(1, new CultureInfo("ru-RU")));
                            fieldDescString = IzumiReplyMessage.FieldNeedWatering.Parse();

                            break;
                        case FieldState.Watered:

                            fieldNameString =
                                $"{_emotes.GetEmoteOrBlank(seed.Name)} {_local.Localize(seed.Name)}, " +
                                IzumiReplyMessage.FieldProgress.Parse(field.ReGrowth
                                    ? (seed.ReGrowth - field.Progress).Days().Humanize(1, new CultureInfo("ru-RU"))
                                    : (seed.Growth - field.Progress).Days().Humanize(1, new CultureInfo("ru-RU")));
                            fieldDescString = IzumiReplyMessage.FieldDontNeedWatering.Parse();

                            break;
                        case FieldState.Completed:

                            // получаем урожай который вырастает из этих семян
                            var crop = await _mediator.Send(new GetCropBySeedIdQuery(seed.Id));

                            fieldNameString =
                                $"{_emotes.GetEmoteOrBlank(crop.Name)} {_local.Localize(crop.Name)}, " +
                                IzumiReplyMessage.FieldCompletedFieldName.Parse();
                            fieldDescString = seed.ReGrowth > 0
                                ? IzumiReplyMessage.FieldInfoStateCompletedReGrowth.Parse(
                                    seed.ReGrowth.Days().Humanize(1, new CultureInfo("ru-RU")))
                                : IzumiReplyMessage.FieldCompletedFieldDesc.Parse();

                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    embed.AddField($"{_emotes.GetEmoteOrBlank("List")} `{field.FieldId}` {fieldNameString}",
                        fieldDescString);
                }
            }

            await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));

            // проверяем нужно ли двинуть прогресс обучения пользователя
            await _mediator.Send(new CheckUserTutorialStepCommand(
                (long) context.User.Id, TutorialStep.CheckHarvestingField));

            await Task.CompletedTask;
        }
    }
}
