using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.RarityEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.AlcoholService;
using Hinode.Izumi.Services.RpgServices.CollectionService;
using Hinode.Izumi.Services.RpgServices.CraftingService;
using Hinode.Izumi.Services.RpgServices.CropService;
using Hinode.Izumi.Services.RpgServices.DrinkService;
using Hinode.Izumi.Services.RpgServices.FishService;
using Hinode.Izumi.Services.RpgServices.FoodService;
using Hinode.Izumi.Services.RpgServices.GatheringService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.ProductService;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserCollectionCommands.
    UserCollectionCategoryCommand
{
    [InjectableService]
    public class UserCollectionCategoryCommand : IUserCollectionCategoryCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IImageService _imageService;
        private readonly ICollectionService _collectionService;
        private readonly IGatheringService _gatheringService;
        private readonly IProductService _productService;
        private readonly ICraftingService _craftingService;
        private readonly IAlcoholService _alcoholService;
        private readonly IDrinkService _drinkService;
        private readonly IFishService _fishService;
        private readonly IFoodService _foodService;
        private readonly ILocalizationService _local;
        private readonly ICropService _cropService;

        public UserCollectionCategoryCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IImageService imageService, ICollectionService collectionService, IGatheringService gatheringService,
            IProductService productService, ICraftingService craftingService, IAlcoholService alcoholService,
            IDrinkService drinkService, IFishService fishService, IFoodService foodService, ILocalizationService local,
            ICropService cropService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _imageService = imageService;
            _collectionService = collectionService;
            _gatheringService = gatheringService;
            _productService = productService;
            _craftingService = craftingService;
            _alcoholService = alcoholService;
            _drinkService = drinkService;
            _fishService = fishService;
            _foodService = foodService;
            _local = local;
            _cropService = cropService;
        }

        public async Task Execute(SocketCommandContext context, CollectionCategory category)
        {
            // получаем все иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем собранную пользователем коллекцию по этой категории
            var userCollection = await _collectionService.GetUserCollection((long) context.User.Id, category);

            var embed = new EmbedBuilder()
                // баннер коллекции
                .WithImageUrl(await _imageService.GetImageUrl(Image.Collection));

            // добавляем embed field в зависимости от категории коллекции
            switch (category)
            {
                case CollectionCategory.Gathering:

                    var gatheringString = string.Empty;
                    // получаем все собирательские ресурсы
                    var gatherings = await _gatheringService.GetAllGatherings();
                    foreach (var gathering in gatherings)
                    {
                        // проверяем есть ли в колекции пользователя этот собирательский ресурс
                        var collection = userCollection.FirstOrDefault(x => x.ItemId == gathering.Id);
                        gatheringString +=
                            // добавляем строку в зависимости от наличия в коллекции
                            $"{emotes.GetEmoteOrBlank(collection == null ? gathering.Name + "BW" : gathering.Name)} {_local.Localize(gathering.Name)}, ";
                    }

                    embed.AddField(category.Localize(),
                        gatheringString.Remove(gatheringString.Length - 2));

                    break;
                case CollectionCategory.Crafting:

                    var craftingString = string.Empty;
                    // получаем все изготавливаемые предметы
                    var craftings = await _craftingService.GetAllCraftings();
                    foreach (var crafting in craftings)
                    {
                        // проверяем есть ли в колекции пользователя этот изготавливаемый предмет
                        var collection = userCollection.FirstOrDefault(x => x.ItemId == crafting.Id);
                        craftingString +=
                            // добавляем строку в зависимости от наличия в коллекции
                            $"{emotes.GetEmoteOrBlank(collection == null ? crafting.Name + "BW" : crafting.Name)} {_local.Localize(crafting.Name)}, ";
                    }

                    embed.AddField(category.Localize(),
                        craftingString.Remove(craftingString.Length - 2));

                    break;
                case CollectionCategory.Alcohol:

                    var alcoholString = string.Empty;
                    // получаем весь алкоголь
                    var alcohols = await _alcoholService.GetAllAlcohol();
                    foreach (var alcohol in alcohols)
                    {
                        // проверяем есть ли в колекции пользователя этот алкоголь
                        var collection = userCollection.FirstOrDefault(x => x.ItemId == alcohol.Id);
                        alcoholString +=
                            // добавляем строку в зависимости от наличия в коллекции
                            $"{emotes.GetEmoteOrBlank(collection == null ? alcohol.Name + "BW" : alcohol.Name)} {_local.Localize(alcohol.Name)}, ";
                    }

                    embed.AddField(category.Localize(),
                        alcoholString.Remove(alcoholString.Length - 2));

                    break;
                case CollectionCategory.Drink:

                    var drinkString = string.Empty;
                    // получаем все напитки
                    var drinks = await _drinkService.GetAllDrinks();
                    foreach (var drink in drinks)
                    {
                        // проверяем есть ли в колекции пользователя этот напиток
                        var collection = userCollection.FirstOrDefault(x => x.ItemId == drink.Id);
                        drinkString +=
                            // добавляем строку в зависимости от наличия в коллекции
                            $"{emotes.GetEmoteOrBlank(collection == null ? drink.Name + "BW" : drink.Name)} {_local.Localize(drink.Name)}, ";
                    }

                    embed.AddField(category.Localize(),
                        drinkString.Remove(drinkString.Length - 2));

                    break;
                case CollectionCategory.Crop:

                    var springCropString = string.Empty;
                    var summerCropString = string.Empty;
                    var autumnCropString = string.Empty;
                    // получаем весь урожай
                    var crops = await _cropService.GetAllCrops();
                    foreach (var crop in crops)
                    {
                        // проверяем есть ли в колекции пользователя этот урожай
                        var collection = userCollection.FirstOrDefault(x => x.ItemId == crop.Id);
                        var displayString =
                            // добавляем строку в зависимости от наличия в коллекции
                            $"{emotes.GetEmoteOrBlank(collection == null ? crop.Name + "BW" : crop.Name)} {_local.Localize(crop.Name)}, ";

                        // определяем в какой сезон нужно добавить запись о коллекции
                        switch (crop.Season)
                        {
                            case Season.Spring:
                                springCropString += displayString;
                                break;
                            case Season.Summer:
                                summerCropString += displayString;
                                break;
                            case Season.Autumn:
                                autumnCropString += displayString;
                                break;
                            case Season.Winter:
                            case Season.Any:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }

                    embed
                        // осенняя коллекция
                        .AddField(IzumiReplyMessage.UserCropsSpringFieldName.Parse(),
                            springCropString.Remove(springCropString.Length - 2))
                        // летняя коллекция
                        .AddField(IzumiReplyMessage.UserCropsSummerFieldName.Parse(),
                            summerCropString.Remove(summerCropString.Length - 2))
                        // весенняя коллекция
                        .AddField(IzumiReplyMessage.UserCropsAutumnFieldName.Parse(),
                            autumnCropString.Remove(autumnCropString.Length - 2));

                    break;
                case CollectionCategory.Fish:

                    var commonFishCollectionString = string.Empty;
                    var rareFishCollectionString = string.Empty;
                    var epicFishCollectionString = string.Empty;
                    var mythicalFishCollectionString = string.Empty;
                    var legendaryFishCollectionString = string.Empty;
                    var divineFishCollectionString = string.Empty;
                    // получаем всю рыбу
                    var fishes = await _fishService.GetAllFish();
                    foreach (var fish in fishes)
                    {
                        // проверяем есть ли в колекции пользователя эта рыба
                        var collection = userCollection.FirstOrDefault(x => x.ItemId == fish.Id);
                        var displayString =
                            // добавляем строку в зависимости от наличия в коллекции
                            $"{emotes.GetEmoteOrBlank(collection == null ? fish.Name + "BW" : fish.Name)} {_local.Localize(fish.Name)}, ";

                        switch (fish.Rarity)
                        {
                            case FishRarity.Common:
                                commonFishCollectionString += displayString;
                                break;
                            case FishRarity.Rare:
                                rareFishCollectionString += displayString;
                                break;
                            case FishRarity.Epic:
                                epicFishCollectionString += displayString;
                                break;
                            case FishRarity.Mythical:
                                mythicalFishCollectionString += displayString;
                                break;
                            case FishRarity.Legendary:
                                legendaryFishCollectionString += displayString;
                                break;
                            case FishRarity.Divine:
                                divineFishCollectionString += displayString;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }

                    embed
                        // обычная
                        .AddField(FishRarity.Common.Localize(),
                            commonFishCollectionString.Remove(commonFishCollectionString.Length - 2))
                        // редкая
                        .AddField(FishRarity.Rare.Localize(),
                            rareFishCollectionString.Remove(rareFishCollectionString.Length - 2))
                        // эпическая
                        .AddField(FishRarity.Epic.Localize(),
                            epicFishCollectionString.Remove(epicFishCollectionString.Length - 2))
                        // мифическая
                        .AddField(FishRarity.Mythical.Localize(),
                            mythicalFishCollectionString.Remove(mythicalFishCollectionString.Length - 2))
                        // легендарная
                        .AddField(FishRarity.Legendary.Localize(),
                            legendaryFishCollectionString.Remove(legendaryFishCollectionString.Length - 2))
                        // божественная
                        .AddField(FishRarity.Divine.Localize(),
                            divineFishCollectionString.Remove(divineFishCollectionString.Length - 2));

                    break;
                case CollectionCategory.Food:

                    var foodMastery0CollectionString = string.Empty;
                    var foodMastery50CollectionString = string.Empty;
                    var foodMastery100CollectionString = string.Empty;
                    var foodMastery150CollectionString = string.Empty;
                    var foodMastery200CollectionString = string.Empty;
                    var foodMastery250CollectionString = string.Empty;
                    // получаем все блюда
                    var foods = await _foodService.GetAllFood();
                    foreach (var food in foods)
                    {
                        // проверяем есть ли в колекции пользователя это блюдо
                        var collection = userCollection.FirstOrDefault(x => x.ItemId == food.Id);
                        var displayString =
                            // добавляем строку в зависимости от наличия в коллекции
                            $"{emotes.GetEmoteOrBlank(collection == null ? food.Name + "BW" : food.Name)} {_local.Localize(food.Name)}, ";

                        // определяем в какую категорию мастерства нужно добавить запись о коллекции
                        switch (food.Mastery)
                        {
                            case 0:
                                foodMastery0CollectionString += displayString;
                                break;
                            case 50:
                                foodMastery50CollectionString += displayString;
                                break;
                            case 100:
                                foodMastery100CollectionString += displayString;
                                break;
                            case 150:
                                foodMastery150CollectionString += displayString;
                                break;
                            case 200:
                                foodMastery200CollectionString += displayString;
                                break;
                            case 250:
                                foodMastery250CollectionString += displayString;
                                break;
                        }
                    }

                    embed
                        // блюда начинающего повара
                        .AddField(IzumiReplyMessage.UserFoodMastery0.Parse(),
                            foodMastery0CollectionString.Remove(foodMastery0CollectionString.Length - 2))
                        // блюда повара-ученика
                        .AddField(IzumiReplyMessage.UserFoodMastery50.Parse(),
                            foodMastery50CollectionString.Remove(foodMastery50CollectionString.Length - 2))
                        // блюда опытного повара
                        .AddField(IzumiReplyMessage.UserFoodMastery100.Parse(),
                            foodMastery100CollectionString.Remove(foodMastery100CollectionString.Length - 2))
                        // блюда повара-профессионала
                        .AddField(IzumiReplyMessage.UserFoodMastery150.Parse(),
                            foodMastery150CollectionString.Remove(foodMastery150CollectionString.Length - 2))
                        // блюда повара-эксперта
                        .AddField(IzumiReplyMessage.UserFoodMastery200.Parse(),
                            foodMastery200CollectionString.Remove(foodMastery200CollectionString.Length - 2))
                        // блюда мастера-повара
                        .AddField(IzumiReplyMessage.UserFoodMastery250.Parse(),
                            foodMastery250CollectionString.Remove(foodMastery250CollectionString.Length - 2));

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(category), category, null);
            }

            await _discordEmbedService.SendEmbed(context.User, embed);
            await Task.CompletedTask;
        }
    }
}
