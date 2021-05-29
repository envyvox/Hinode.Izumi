using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.InventoryService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.TutorialService.Commands;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.UserInfoCommands.UserInventoryCommands.UserInventoryCommand
{
    [InjectableService]
    public class UserInventoryCommand : IUserInventoryCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public UserInventoryCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем все иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем валюту пользователя
            var userCurrency = await _mediator.Send(new GetUserCurrenciesQuery((long) context.User.Id));
            // получаем собирательские предметы пользователя
            var userGathering = await _mediator.Send(new GetUserGatheringsQuery((long) context.User.Id));
            // получаем продукты пользователя
            var userProduct = await _mediator.Send(new GetUserProductsQuery((long) context.User.Id));
            // получаем предметы изготовления пользователя
            var userCrafting = await _mediator.Send(new GetUserCraftingsQuery((long) context.User.Id));
            // получаем алкоголь пользователя
            var userAlcohol = await _mediator.Send(new GetUserAlcoholsQuery((long) context.User.Id));
            // получаем напитки пользователя
            var userDrink = await _mediator.Send(new GetUserDrinksQuery((long) context.User.Id));
            // получаем семена пользователя
            var userSeeds = await _mediator.Send(new GetUserSeedsQuery((long) context.User.Id));
            // получаем урожай пользователя
            var userCrops = await _mediator.Send(new GetUserCropsQuery((long) context.User.Id));
            // получаем рыбу пользователя
            var userFish = await _mediator.Send(new GetUserFishesQuery((long) context.User.Id));
            // получаем еду пользователя
            var userFood = await _mediator.Send(new GetUserFoodsQuery((long) context.User.Id));
            // получаем коробки пользователя
            var userBox = await _mediator.Send(new GetUserBoxesQuery((long) context.User.Id));

            var embed = new EmbedBuilder()
                // баннер инвентаря
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.Inventory)))
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
                            ? $"{emotes.GetEmoteOrBlank(food.Name)} {food.Amount} {_local.Localize(LocalizationCategory.Food, food.FoodId, food.Amount)}, "
                            : "")),

                    InventoryCategory.Box => Enum.GetValues(typeof(Box))
                        .Cast<Box>()
                        .Where(box => userBox.ContainsKey(box))
                        .Aggregate(string.Empty, (current, box) =>
                            current +
                            $"{emotes.GetEmoteOrBlank(box.Emote())} {userBox[box].Amount} {_local.Localize(box.ToString(), userBox[box].Amount)}, "),

                    // TODO ADD SEAFOOD DISPLAY
                    InventoryCategory.Seafood => "",

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

            await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
            // проверяем нужно ли двинуть прогресс обучения пользователя
            await _mediator.Send(new CheckUserTutorialStepCommand((long) context.User.Id, TutorialStep.CheckInventory));
            await Task.CompletedTask;
        }
    }
}
