using System;
using System.Threading.Tasks;
using Discord;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.AchievementEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.AchievementService;
using Hinode.Izumi.Services.RpgServices.AlcoholService;
using Hinode.Izumi.Services.RpgServices.CalculationService;
using Hinode.Izumi.Services.RpgServices.CollectionService;
using Hinode.Izumi.Services.RpgServices.CraftingService;
using Hinode.Izumi.Services.RpgServices.FoodService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.InventoryService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.LocationService;
using Hinode.Izumi.Services.RpgServices.MasteryService;
using Hinode.Izumi.Services.RpgServices.StatisticService;
using Hinode.Izumi.Services.RpgServices.TrainingService;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.BackgroundJobs.MakingJob
{
    [InjectableService]
    public class MakingJob : IMakingJob
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IDiscordGuildService _discordGuildService;
        private readonly IEmoteService _emoteService;
        private readonly ILocalizationService _local;
        private readonly IStatisticService _statisticService;
        private readonly IAchievementService _achievementService;
        private readonly IMasteryService _masteryService;
        private readonly IInventoryService _inventoryService;
        private readonly IImageService _imageService;
        private readonly ICollectionService _collectionService;
        private readonly IFoodService _foodService;
        private readonly ILocationService _locationService;
        private readonly ICalculationService _calc;
        private readonly ICraftingService _craftingService;
        private readonly IAlcoholService _alcoholService;
        private readonly ITrainingService _trainingService;

        public MakingJob(IDiscordEmbedService discordEmbedService, IDiscordGuildService discordGuildService,
            IEmoteService emoteService, ILocalizationService local, IStatisticService statisticService,
            IAchievementService achievementService, IMasteryService masteryService, IInventoryService inventoryService,
            IImageService imageService, ICollectionService collectionService, IFoodService foodService,
            ILocationService locationService, ICalculationService calc, ICraftingService craftingService,
            IAlcoholService alcoholService, ITrainingService trainingService)
        {
            _discordEmbedService = discordEmbedService;
            _discordGuildService = discordGuildService;
            _emoteService = emoteService;
            _local = local;
            _statisticService = statisticService;
            _achievementService = achievementService;
            _masteryService = masteryService;
            _inventoryService = inventoryService;
            _imageService = imageService;
            _collectionService = collectionService;
            _foodService = foodService;
            _locationService = locationService;
            _calc = calc;
            _craftingService = craftingService;
            _alcoholService = alcoholService;
            _trainingService = trainingService;
        }

        public async Task CompleteCrafting(long userId, long craftingId, long amount, Location location)
        {
            // получаем изготавливаемый предмет
            var crafting = await _craftingService.GetCrafting(craftingId);
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем мастерство изготовления пользователя и округляем его
            var userMastery = (long) Math.Floor(
                (await _masteryService.GetUserMastery(userId, Mastery.Crafting))
                .Amount);
            // определяем количество итоговых изготовленных предметов после проков мастерства
            var amountAfterProcs = await _calc.CraftingAmountAfterMasteryProcs(crafting.Id, userMastery, amount);

            // обновляем текущую локацию пользователя
            await _locationService.UpdateUserLocation(userId, location);
            // удаляем информацию о перемещении
            await _locationService.RemoveUserMovement(userId);
            // добавляем пользователю изготовленные предметы
            await _inventoryService.AddItemToUser(userId, InventoryCategory.Crafting, crafting.Id, amountAfterProcs);
            // добавляем пользователь запись в коллекции
            await _collectionService.AddCollectionToUser(userId, CollectionCategory.Crafting, crafting.Id);
            // добавляем пользователю мастерство изготовления
            await _masteryService.AddMasteryToUser(userId, Mastery.Crafting,
                // определяем количество полученного мастерства изготовления
                await _calc.MasteryXp(MasteryXpProperty.CraftingResource, userMastery, amount));
            // добавляем пользователю статистику созданных предметов
            await _statisticService.AddStatisticToUser(userId, Statistic.CraftingResource, amountAfterProcs);
            // проверяем выполнил ли пользователь достижения
            await _achievementService.CheckAchievement(userId, new[]
            {
                Achievement.FirstCraftResource,
                Achievement.Craft30Resource,
                Achievement.Craft250Resource,
                Achievement.CompleteCollectionCrafting
            });

            var embed = new EmbedBuilder()
                .WithAuthor(Location.MakingCrafting.Localize())
                // баннер изготовления
                .WithImageUrl(await _imageService.GetImageUrl(Image.Crafting))
                // оповещаем о окончании процесса изготовления
                .WithDescription(
                    IzumiReplyMessage.CraftingResourceCompleteDesc.Parse(
                        emotes.GetEmoteOrBlank(crafting.Name), _local.Localize(crafting.Name, 2)) +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // полученные предметы
                .AddField(IzumiReplyMessage.CraftingResourceReceivedFieldName.Parse(),
                    $"{emotes.GetEmoteOrBlank(crafting.Name)} {amountAfterProcs} {_local.Localize(crafting.Name, amountAfterProcs)}");

            await _discordEmbedService.SendEmbed(
                await _discordGuildService.GetSocketUser(userId), embed);
            await Task.CompletedTask;
        }

        public async Task CompleteAlcohol(long userId, long alcoholId, long amount, Location location)
        {
            // получаем изготавливаемый алкоголь
            var alcohol = await _alcoholService.GetAlcohol(alcoholId);
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем мастерство изготовления пользователя и округляем его
            var userMastery = (long) Math.Floor(
                (await _masteryService.GetUserMastery(userId, Mastery.Crafting))
                .Amount);
            // определяем количество изготовленного алкоголя после проков мастерства
            var amountAfterProcs = await _calc.AlcoholAmountAfterMasteryProcs(alcohol.Id, userMastery, amount);

            // обновляем текущую локацию пользователя
            await _locationService.UpdateUserLocation(userId, location);
            // удаляем информацию о перемещении
            await _locationService.RemoveUserMovement(userId);
            // добавляем пользователю изготовленный алкоголь
            await _inventoryService.AddItemToUser(userId, InventoryCategory.Alcohol, alcohol.Id, amountAfterProcs);
            // добавляем пользователю запись в коллекцию
            await _collectionService.AddCollectionToUser(userId, CollectionCategory.Alcohol, alcohol.Id);
            // добавляем пользователю мастерство изготовление
            await _masteryService.AddMasteryToUser(userId, Mastery.Crafting,
                // определяем количество полученного мастерства изготовления
                await _calc.MasteryXp(MasteryXpProperty.CraftingAlcohol, userMastery, amount));
            // добавляем пользователю статистику изготовленного алкоголя
            await _statisticService.AddStatisticToUser(userId, Statistic.CraftingAlcohol, amountAfterProcs);
            // проверяем выполнил ли пользователь достижения
            await _achievementService.CheckAchievement(userId, new[]
            {
                Achievement.FirstCraftAlcohol,
                Achievement.Craft10Alcohol,
                Achievement.Craft80Alcohol,
                Achievement.CompleteCollectionAlcohol
            });

            var embed = new EmbedBuilder()
                .WithAuthor(Location.MakingAlcohol.Localize())
                // баннер изготовления
                .WithImageUrl(await _imageService.GetImageUrl(Image.Crafting))
                // оповещаем о окончании изготовления
                .WithDescription(
                    IzumiReplyMessage.CraftingAlcoholCompleteDesc.Parse(
                        emotes.GetEmoteOrBlank(alcohol.Name), _local.Localize(alcohol.Name)) +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // полученный алкоголь
                .AddField(IzumiReplyMessage.CraftingAlcoholReceivedFieldName.Parse(),
                    $"{emotes.GetEmoteOrBlank(alcohol.Name)} {amountAfterProcs} {_local.Localize(alcohol.Name, amountAfterProcs)}");

            await _discordEmbedService.SendEmbed(
                await _discordGuildService.GetSocketUser(userId), embed);
            await Task.CompletedTask;
        }

        public async Task CompleteFood(long userId, long foodId, long amount, Location location)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем приготавливаемую еду
            var food = await _foodService.GetFood(foodId);
            // получаем мастерство приготовления пользователя и округляем его
            var userMastery = (long) Math.Floor(
                (await _masteryService.GetUserMastery(userId, Mastery.Cooking))
                .Amount);

            // обновляем текущую локацию пользователя
            await _locationService.UpdateUserLocation(userId, location);
            // удаляем информацию о перемещении
            await _locationService.RemoveUserMovement(userId);
            // добавляем пользователю приготовленную еду
            await _inventoryService.AddItemToUser(userId, InventoryCategory.Food, foodId, amount);
            // добавляем пользователю запись в коллекцию
            await _collectionService.AddCollectionToUser(userId, CollectionCategory.Food, foodId);
            // добавляем пользователю мастерство приготовления
            await _masteryService.AddMasteryToUser(userId, Mastery.Cooking,
                // определяем количество полученного мастерства приготовления
                await _calc.MasteryXp(MasteryXpProperty.Cooking, userMastery, amount));
            // добавляем пользователю статистику приготовленного блюда
            await _statisticService.AddStatisticToUser(userId, Statistic.Cooking, amount);
            // добавляем пользователю статистику приготовленного блюда по категории мастерства
            await _statisticService.AddStatisticToUser(userId, food.Mastery switch
            {
                0 => Statistic.CookingBeginner,
                50 => Statistic.CookingApprentice,
                100 => Statistic.CookingExperienced,
                150 => Statistic.CookingProfessional,
                200 => Statistic.CookingExpert,
                250 => Statistic.CookingMaster,
                _ => throw new ArgumentOutOfRangeException()
            }, amount);
            // проверяем выполнил ли пользователь достижения
            await _achievementService.CheckAchievement(userId, new[]
            {
                Achievement.FirstCook,
                Achievement.CompleteCollectionFood
            });

            var embed = new EmbedBuilder()
                .WithAuthor(Location.MakingFood.Localize())
                // баннер приготовления
                .WithImageUrl(await _imageService.GetImageUrl(Image.Cooking))
                // оповещаем о окончании приготовления
                .WithDescription(
                    IzumiReplyMessage.CraftingFoodCompleteDesc.Parse() +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // полученная еда
                .AddField(IzumiReplyMessage.CraftingFoodReceivedFieldName.Parse(),
                    $"{emotes.GetEmoteOrBlank(food.Name)} {amount} {_local.Localize(food.Name)}");

            await _discordEmbedService.SendEmbed(
                await _discordGuildService.GetSocketUser(userId), embed);
            // проверяем нужно ли двинуть прогресс обучения пользователя
            if (food.Id == 4) await _trainingService.CheckStep(userId, TrainingStep.CookFriedEgg);
            await Task.CompletedTask;
        }
    }
}
