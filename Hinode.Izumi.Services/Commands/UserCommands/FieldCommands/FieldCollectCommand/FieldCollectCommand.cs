using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.AchievementEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.AchievementService;
using Hinode.Izumi.Services.RpgServices.CollectionService;
using Hinode.Izumi.Services.RpgServices.CropService;
using Hinode.Izumi.Services.RpgServices.FieldService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.InventoryService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.PropertyService;
using Hinode.Izumi.Services.RpgServices.SeedService;
using Hinode.Izumi.Services.RpgServices.StatisticService;
using Hinode.Izumi.Services.RpgServices.UserService;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.FieldCommands.FieldCollectCommand
{
    [InjectableService]
    public class FieldCollectCommand : IFieldCollectCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IFieldService _fieldService;
        private readonly IPropertyService _propertyService;
        private readonly IImageService _imageService;
        private readonly IInventoryService _inventoryService;
        private readonly IStatisticService _statisticService;
        private readonly ISeedService _seedService;
        private readonly IAchievementService _achievementService;
        private readonly ICollectionService _collectionService;
        private readonly ICropService _cropService;
        private readonly ILocalizationService _local;
        private readonly IUserService _userService;

        public FieldCollectCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IFieldService fieldService, IPropertyService propertyService, IImageService imageService,
            IInventoryService inventoryService, IStatisticService statisticService, ISeedService seedService,
            IAchievementService achievementService, ICollectionService collectionService, ICropService cropService,
            ILocalizationService local, IUserService userService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _fieldService = fieldService;
            _propertyService = propertyService;
            _imageService = imageService;
            _inventoryService = inventoryService;
            _statisticService = statisticService;
            _seedService = seedService;
            _achievementService = achievementService;
            _collectionService = collectionService;
            _cropService = cropService;
            _local = local;
            _userService = userService;
        }

        public async Task Execute(SocketCommandContext context, long fieldId)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем клетку земли участка пользователя
            var userField = await _fieldService.GetUserField((long) context.User.Id, fieldId);

            switch (userField.State)
            {
                case FieldState.Empty:

                    // нельзя собрать с пустой клетки участка
                    await Task.FromException(new Exception(IzumiReplyMessage.UserFieldEmpty.Parse()));

                    break;
                case FieldState.Planted:
                case FieldState.Watered:

                    // нельзя собрать с еще не готовой к сбору клетки участка
                    await Task.FromException(new Exception(IzumiReplyMessage.UserFieldCollectNotReady.Parse()));

                    break;
                case FieldState.Completed:

                    // получаем посаженные семена
                    var seed = await _seedService.GetSeed(userField.SeedId);
                    // получаем урожай из этих семян
                    var crop = await _cropService.GetCropBySeedId(seed.Id);

                    var embed = new EmbedBuilder()
                        // баннер участка
                        .WithImageUrl(await _imageService.GetImageUrl(Image.Field));

                    // определяем количество собранного урожая
                    var amount = seed.Multiply ? new Random().Next(2, 4) : 1;

                    // если семяна имеют повторный рост, нужно запустить рост повторно
                    if (seed.ReGrowth > 0)
                    {
                        // запускаем повторный рост семян
                        await _fieldService.StartReGrowth((long) context.User.Id, userField.FieldId);

                        embed.WithDescription(IzumiReplyMessage.UserFieldCollectSuccessReGrowth.Parse(
                            emotes.GetEmoteOrBlank(crop.Name), amount, _local.Localize(crop.Name, amount),
                            seed.ReGrowth));
                    }
                    else
                    {
                        // сбрасываем состоение клетки участка на пустое
                        await _fieldService.ResetField((long) context.User.Id, userField.FieldId);

                        embed.WithDescription(IzumiReplyMessage.UserFieldCollectSuccess.Parse(
                            emotes.GetEmoteOrBlank(crop.Name), amount, _local.Localize(crop.Name, amount)));
                    }

                    // добавляем пользователю собранный урожай
                    await _inventoryService.AddItemToUser(
                        (long) context.User.Id, InventoryCategory.Crop, crop.Id, amount);
                    // добавляем в коллекцию пользователя урожай
                    await _collectionService.AddCollectionToUser(
                        (long) context.User.Id, CollectionCategory.Crop, crop.Id);
                    // добавляем пользователю статистику собранного урожая
                    await _statisticService.AddStatisticToUser((long) context.User.Id, Statistic.CropHarvested);
                    // проверяем не выполнил ли пользователь достижения
                    await _achievementService.CheckAchievement(
                        (long) context.User.Id, new[]
                        {
                            Achievement.Collect50Crop,
                            Achievement.Collect300Crop,
                            Achievement.CompleteCollectionCrop
                        });
                    // отнимаем энергию у пользователя
                    await _userService.RemoveEnergyFromUser((long) context.User.Id,
                        // получаем количество энергии
                        await _propertyService.GetPropertyValue(Property.EnergyCostFieldCollect));

                    await _discordEmbedService.SendEmbed(context.User, embed);
                    await Task.CompletedTask;

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
