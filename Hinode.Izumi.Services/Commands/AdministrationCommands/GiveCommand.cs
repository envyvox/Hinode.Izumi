using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.AlcoholService;
using Hinode.Izumi.Services.RpgServices.CraftingService;
using Hinode.Izumi.Services.RpgServices.CropService;
using Hinode.Izumi.Services.RpgServices.DrinkService;
using Hinode.Izumi.Services.RpgServices.FishService;
using Hinode.Izumi.Services.RpgServices.FoodService;
using Hinode.Izumi.Services.RpgServices.GatheringService;
using Hinode.Izumi.Services.RpgServices.InventoryService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.ProductService;
using Hinode.Izumi.Services.RpgServices.SeedService;
using Hinode.Izumi.Services.RpgServices.UserService;

namespace Hinode.Izumi.Services.Commands.AdministrationCommands
{
    [IzumiRequireRole(DiscordRole.Administration)]
    public class GiveCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IInventoryService _inventoryService;
        private readonly IUserService _userService;
        private readonly IGatheringService _gatheringService;
        private readonly ICraftingService _craftingService;
        private readonly IProductService _productService;
        private readonly IAlcoholService _alcoholService;
        private readonly IDrinkService _drinkService;
        private readonly IFoodService _foodService;
        private readonly IFishService _fishService;
        private readonly ISeedService _seedService;
        private readonly ICropService _cropService;
        private readonly ILocalizationService _local;
        private readonly IDiscordGuildService _discordGuildService;

        public GiveCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IInventoryService inventoryService, IUserService userService, IGatheringService gatheringService,
            ICraftingService craftingService, IProductService productService, IAlcoholService alcoholService,
            IDrinkService drinkService, IFoodService foodService, IFishService fishService,
            ISeedService seedService, ICropService cropService, ILocalizationService local,
            IDiscordGuildService discordGuildService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _inventoryService = inventoryService;
            _userService = userService;
            _gatheringService = gatheringService;
            _craftingService = craftingService;
            _productService = productService;
            _alcoholService = alcoholService;
            _drinkService = drinkService;
            _foodService = foodService;
            _fishService = fishService;
            _seedService = seedService;
            _cropService = cropService;
            _local = local;
            _discordGuildService = discordGuildService;
        }

        [Command("give")]
        public async Task GiveCommandTask(long userId, InventoryCategory category, long itemId, long amount) =>
            await GiveTask(userId, category, itemId, amount);

        [Command("give")]
        public async Task GiveCommandTask(IUser user, InventoryCategory category, long itemId, long amount) =>
            await GiveTask((long) user.Id, category, itemId, amount);

        private async Task GiveTask(long userId, InventoryCategory category, long itemId, long amount)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем пользователя
            var user = await _userService.GetUser(userId);
            // добавляем ему предмет
            await _inventoryService.AddItemToUser(user.Id, category, itemId, amount);

            // заполняем название предмета
            string itemName;
            // заполняем название иконки предмета
            var emoteName = string.Empty;
            switch (category)
            {
                case InventoryCategory.Currency:
                    itemName = ((Currency) itemId).ToString();
                    break;
                case InventoryCategory.Box:
                    itemName = ((Box) itemId).ToString();
                    emoteName = ((Box) itemId).Emote();
                    break;
                case InventoryCategory.Gathering:
                    var gathering = await _gatheringService.GetGathering(itemId);
                    itemName = gathering.Name;
                    break;
                case InventoryCategory.Product:
                    var product = await _productService.GetProduct(itemId);
                    itemName = product.Name;
                    break;
                case InventoryCategory.Crafting:
                    var crafting = await _craftingService.GetCrafting(itemId);
                    itemName = crafting.Name;
                    break;
                case InventoryCategory.Alcohol:
                    var alcohol = await _alcoholService.GetAlcohol(itemId);
                    itemName = alcohol.Name;
                    break;
                case InventoryCategory.Drink:
                    var drink = await _drinkService.GetDrink(itemId);
                    itemName = drink.Name;
                    break;
                case InventoryCategory.Seed:
                    var seed = await _seedService.GetSeed(itemId);
                    itemName = seed.Name;
                    break;
                case InventoryCategory.Crop:
                    var crop = await _cropService.GetCrop(itemId);
                    itemName = crop.Name;
                    break;
                case InventoryCategory.Fish:
                    var fish = await _fishService.GetFish(itemId);
                    itemName = fish.Name;
                    break;
                case InventoryCategory.Food:
                    var food = await _foodService.GetFood(itemId);
                    itemName = food.Name;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(category), category, null);
            }

            // если название иконки предмета не было заполнено выше - то оно такое же как и название предмета
            if (emoteName.Length < 1) emoteName = itemName;

            var embed = new EmbedBuilder()
                // подтверждаем выдачу предмета
                .WithDescription(IzumiReplyMessage.AdmGiveDesc.Parse(
                    emotes.GetEmoteOrBlank(user.Title.Emote()), user.Title.Localize(), user.Name,
                    emotes.GetEmoteOrBlank(emoteName), amount, _local.Localize(itemName, amount)));

            await _discordEmbedService.SendEmbed(Context.Channel, embed);

            var embedPm = new EmbedBuilder()
                // оповещаем пользователя о выдаче предмета
                .WithDescription(IzumiReplyMessage.AdmGivePmDesc.Parse(
                    emotes.GetEmoteOrBlank(emoteName), amount, _local.Localize(itemName, amount)));

            await _discordEmbedService.SendEmbed(
                await _discordGuildService.GetSocketUser(user.Id), embedPm);
        }
    }
}
