using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.InventoryService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.TrainingService;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserInventoryCommands.UserInventoryCommand
{
    [InjectableService]
    public class UserInventoryCommand : IUserInventoryCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly ILocalizationService _local;
        private readonly ITrainingService _trainingService;
        private readonly IInventoryService _inventoryService;
        private readonly IImageService _imageService;


        public UserInventoryCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            ILocalizationService local, ITrainingService trainingService, IInventoryService inventoryService,
            IImageService imageService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _local = local;
            _trainingService = trainingService;
            _inventoryService = inventoryService;
            _imageService = imageService;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем все иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем валюту пользователя
            var userCurrency = await _inventoryService.GetUserCurrency((long) context.User.Id);
            // получаем собирательские предметы пользователя
            var userGathering = await _inventoryService.GetUserGathering((long) context.User.Id);
            // получаем продукты пользователя
            var userProduct = await _inventoryService.GetUserProduct((long) context.User.Id);
            // получаем предметы изготовления пользователя
            var userCrafting = await _inventoryService.GetUserCrafting((long) context.User.Id);
            // получаем алкоголь пользователя
            var userAlcohol = await _inventoryService.GetUserAlcohol((long) context.User.Id);
            // получаем напитки пользователя
            var userDrink = await _inventoryService.GetUserDrink((long) context.User.Id);
            // получаем семена пользователя
            var userSeeds = await _inventoryService.GetUserSeed((long) context.User.Id);
            // получаем урожай пользователя
            var userCrops = await _inventoryService.GetUserCrop((long) context.User.Id);
            // получаем рыбу пользователя
            var userFish = await _inventoryService.GetUserFish((long) context.User.Id);
            // получаем еду пользователя
            var userFood = await _inventoryService.GetUserFood((long) context.User.Id);
            // получаем коробки пользователя
            var userBox = await _inventoryService.GetUserBox((long) context.User.Id);

            var embed = new EmbedBuilder()
                // баннер инвентаря
                .WithImageUrl(await _imageService.GetImageUrl(Image.Inventory))
                .WithDescription(IzumiReplyMessage.InventoryDesc.Parse());

            // для каждой категории предметов создаем embed field
            foreach (var category in Enum.GetValues(typeof(InventoryCategory))
                .Cast<InventoryCategory>())
            {
                // заполняем строку предметов по категории по шаблону (иконка, количество, название) через запятую
                var groupString = category switch
                {
                    InventoryCategory.Currency => Enum.GetValues(typeof(Currency))
                        .Cast<Currency>()
                        .Where(currency => userCurrency.ContainsKey(currency))
                        .Aggregate(string.Empty, (current, currency) =>
                            current +
                            $"{emotes.GetEmoteOrBlank(currency.ToString())} {userCurrency[currency].Amount} {_local.Localize(currency.ToString(), userCurrency[currency].Amount)}, "),

                    InventoryCategory.Gathering => userGathering.Aggregate(string.Empty, (current, gathering) =>
                        current + (gathering.Amount > 0
                            ? $"{emotes.GetEmoteOrBlank(gathering.Name)} {gathering.Amount} {_local.Localize(gathering.Name, gathering.Amount)}, "
                            : "")),

                    InventoryCategory.Product => userProduct.Aggregate(string.Empty, (current, product) =>
                        current + (product.Amount > 0
                            ? $"{emotes.GetEmoteOrBlank(product.Name)} {product.Amount} {_local.Localize(product.Name, product.Amount)}, "
                            : "")),

                    InventoryCategory.Crafting => userCrafting.Aggregate(string.Empty, (current, crafting) =>
                        current + (crafting.Amount > 0
                            ? $"{emotes.GetEmoteOrBlank(crafting.Name)} {crafting.Amount} {_local.Localize(crafting.Name, crafting.Amount)}, "
                            : "")),

                    InventoryCategory.Alcohol => userAlcohol.Aggregate(string.Empty, (current, alcohol) =>
                        current + (alcohol.Amount > 0
                            ? $"{emotes.GetEmoteOrBlank(alcohol.Name)} {alcohol.Amount} {_local.Localize(alcohol.Name, alcohol.Amount)}, "
                            : "")),

                    InventoryCategory.Drink => userDrink.Aggregate(string.Empty, (current, drink) =>
                        current + (drink.Amount > 0
                            ? $"{emotes.GetEmoteOrBlank(drink.Name)} {drink.Amount} {_local.Localize(drink.Name, drink.Amount)}, "
                            : "")),

                    InventoryCategory.Seed => userSeeds.Aggregate(string.Empty, (current, seed) =>
                        current + (seed.Amount > 0
                            ? $"{emotes.GetEmoteOrBlank(seed.Name)} {seed.Amount} {_local.Localize(seed.Name, seed.Amount)}, "
                            : "")),

                    InventoryCategory.Crop => userCrops.Aggregate(string.Empty, (current, crop) =>
                        current + (crop.Amount > 0
                            ? $"{emotes.GetEmoteOrBlank(crop.Name)} {crop.Amount} {_local.Localize(crop.Name, crop.Amount)}, "
                            : "")),

                    InventoryCategory.Fish => userFish.Aggregate(string.Empty, (current, fish) =>
                        current + (fish.Amount > 0
                            ? $"{emotes.GetEmoteOrBlank(fish.Name)} {fish.Amount} {_local.Localize(fish.Name, fish.Amount)}, "
                            : "")),

                    InventoryCategory.Food => userFood.Aggregate(string.Empty, (current, food) =>
                        current + (food.Amount > 0
                            ? $"{emotes.GetEmoteOrBlank(food.Name)} {food.Amount} {_local.Localize(food.Name, food.Amount)}, "
                            : "")),

                    InventoryCategory.Box => Enum.GetValues(typeof(Box))
                        .Cast<Box>()
                        .Where(box => userBox.ContainsKey(box))
                        .Aggregate(string.Empty, (current, box) =>
                            current +
                            $"{emotes.GetEmoteOrBlank(box.Emote())} {userBox[box].Amount} {_local.Localize(box.ToString(), userBox[box].Amount)}, "),

                    _ => throw new ArgumentOutOfRangeException()
                };

                // некоторые категории имеют слишком много предметов, чтобы отображать их всех в одном embed field
                // если такое происходит - вместо отображения всех предметов, предлагаем пользователю просмотреть их
                // в отдельной команде
                var outOfLimitString = category switch
                {
                    InventoryCategory.Seed => IzumiReplyMessage.InventorySeedOutOfLimit.Parse(),
                    InventoryCategory.Crop => IzumiReplyMessage.InventoryCropOutOfLimit.Parse(),
                    InventoryCategory.Fish => IzumiReplyMessage.InventoryFishOutOfLimit.Parse(),
                    InventoryCategory.Food => IzumiReplyMessage.InventoryFoodOutOfLimit.Parse(),
                    _ => ""
                };

                if (groupString.Length > 0)
                    embed.AddField(category.Localize(),
                        groupString.Length > 1024
                            ? outOfLimitString
                            : groupString.Remove(groupString.Length - 2) + (category == InventoryCategory.Box
                                ? IzumiReplyMessage.InventoryBoxCommand.Parse()
                                : "")
                    );
            }

            await _discordEmbedService.SendEmbed(context.User, embed);
            // проверяем нужно ли двинуть прогресс обучения пользователя
            await _trainingService.CheckStep((long) context.User.Id, TrainingStep.CheckInventory);
            await Task.CompletedTask;
        }
    }
}
