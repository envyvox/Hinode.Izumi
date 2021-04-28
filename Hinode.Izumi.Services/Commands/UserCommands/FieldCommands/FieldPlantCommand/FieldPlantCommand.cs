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
using Hinode.Izumi.Services.RpgServices.FieldService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.InventoryService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.PropertyService;
using Hinode.Izumi.Services.RpgServices.SeedService;
using Hinode.Izumi.Services.RpgServices.StatisticService;
using Hinode.Izumi.Services.RpgServices.UserService;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.FieldCommands.FieldPlantCommand
{
    [InjectableService]
    public class FieldPlantCommand : IFieldPlantCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IFieldService _fieldService;
        private readonly ILocalizationService _local;
        private readonly ISeedService _seedService;
        private readonly IImageService _imageService;
        private readonly IPropertyService _propertyService;
        private readonly IUserService _userService;
        private readonly IAchievementService _achievementService;
        private readonly IStatisticService _statisticService;
        private readonly IInventoryService _inventoryService;

        public FieldPlantCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IFieldService fieldService, ILocalizationService local, ISeedService seedService,
            IImageService imageService, IPropertyService propertyService, IUserService userService,
            IAchievementService achievementService, IStatisticService statisticService,
            IInventoryService inventoryService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _fieldService = fieldService;
            _local = local;
            _seedService = seedService;
            _imageService = imageService;
            _propertyService = propertyService;
            _userService = userService;
            _achievementService = achievementService;
            _statisticService = statisticService;
            _inventoryService = inventoryService;
        }

        public async Task Execute(SocketCommandContext context, long fieldId, string seedNamePattern)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем клетку участка пользователя
            var userField = await _fieldService.GetUserField((long) context.User.Id, fieldId);

            switch (userField.State)
            {
                case FieldState.Planted:
                case FieldState.Watered:

                    // если клетка не пустая - на нее нельзя ничего посадить
                    await Task.FromException(new Exception(IzumiReplyMessage.UserFieldPlantAlready.Parse()));

                    break;
                case FieldState.Completed:

                    // если на клетке уже вырос урожай - его нужно сперва собрать
                    await Task.FromException(new Exception(IzumiReplyMessage.UserFieldCompleted.Parse()));

                    break;
                case FieldState.Empty:

                    // получаем семена по локализированному названию
                    var seed = await _seedService.GetSeed(seedNamePattern);
                    // получаем эти семена у пользователя
                    var userSeed = await _inventoryService.GetUserSeed((long) context.User.Id, seed.Id);
                    // получаем текущий сезон в мире
                    var season = (Season) await _propertyService.GetPropertyValue(Property.CurrentSeason);

                    // проверяем есть ли у пользователя необходимые семена
                    if (userSeed.Amount < 1)
                    {
                        await Task.FromException(new Exception(IzumiReplyMessage.UserDontHaveSeed.Parse(
                            emotes.GetEmoteOrBlank(seed.Name), _local.Localize(seed.Name),
                            Location.Capital.Localize(true))));
                    }
                    // проверяем подходит ли сезон семян для посадки
                    else if (seed.Season != season)
                    {
                        await Task.FromException(
                            new Exception(IzumiReplyMessage.UserFieldPlantOnlyCurrentSeason.Parse()));
                    }
                    else
                    {
                        // садим семена на эту клетку участка
                        await _fieldService.Plant((long) context.User.Id, fieldId, seed.Id);

                        // забираем у пользователя посаженные семена
                        await _inventoryService.RemoveItemFromUser(
                            (long) context.User.Id, InventoryCategory.Seed, seed.Id);
                        // добавляем пользователю статистку посаженных семян
                        await _statisticService.AddStatisticToUser((long) context.User.Id, Statistic.SeedPlanted);
                        // проверяем не выполнил ли пользователь достижения
                        await _achievementService.CheckAchievement(
                            (long) context.User.Id, new[]
                            {
                                Achievement.FirstPlant,
                                Achievement.Plant25Seed,
                                Achievement.Plant150Seed
                            });
                        // отнимаем энергию у пользователя
                        await _userService.RemoveEnergyFromUser((long) context.User.Id,
                            // получаем количество энергии
                            await _propertyService.GetPropertyValue(Property.EnergyCostFieldPlant));

                        var embed = new EmbedBuilder()
                            // баннер участка
                            .WithImageUrl(await _imageService.GetImageUrl(Image.Field))
                            // подверждаем успешную посадку семян
                            .WithDescription(IzumiReplyMessage.UserFieldPlantSuccess.Parse(
                                emotes.GetEmoteOrBlank(seed.Name), _local.Localize(seed.Name), seed.Growth));

                        await _discordEmbedService.SendEmbed(context.User, embed);
                        await Task.CompletedTask;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
