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
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.EmoteService.Models;
using Hinode.Izumi.Services.RpgServices.CropService;
using Hinode.Izumi.Services.RpgServices.FieldService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.PropertyService;
using Hinode.Izumi.Services.RpgServices.SeedService;
using Hinode.Izumi.Services.RpgServices.TrainingService;
using Humanizer;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.FieldCommands.FieldInfoCommand
{
    [InjectableService]
    public class FieldInfoCommand : IFieldInfoCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IImageService _imageService;
        private readonly IPropertyService _propertyService;
        private readonly ILocalizationService _local;
        private readonly ITrainingService _trainingService;
        private readonly IFieldService _fieldService;
        private readonly ISeedService _seedService;
        private readonly ICropService _cropService;

        private Dictionary<string, EmoteModel> _emotes;

        public FieldInfoCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IImageService imageService, IPropertyService propertyService, ILocalizationService local,
            ITrainingService trainingService, IFieldService fieldService, ISeedService seedService,
            ICropService cropService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _imageService = imageService;
            _propertyService = propertyService;
            _local = local;
            _trainingService = trainingService;
            _fieldService = fieldService;
            _seedService = seedService;
            _cropService = cropService;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            _emotes = await _emoteService.GetEmotes();
            // получаем участок пользователя
            var userFields = await _fieldService.GetUserField((long) context.User.Id);

            var embed = new EmbedBuilder()
                // баннер участка
                .WithImageUrl(await _imageService.GetImageUrl(Image.Field));

            // если у пользователя нет клеток участка, предлагаем ему купить участок
            if (userFields.Length < 1)
            {
                // получаем стоимость участка земли
                var fieldPrice = await _propertyService.GetPropertyValue(Property.FieldPrice);
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
                    var seed = await _seedService.GetSeed(field.SeedId);

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
                            var crop = await _cropService.GetCropBySeedId(seed.Id);

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

            await _discordEmbedService.SendEmbed(context.User, embed);
            // проверяем нужно ли двинуть прогресс обучения пользователя
            await _trainingService.CheckStep((long) context.User.Id, TrainingStep.CheckHarvestingField);
        }
    }
}
