using System;
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
using Hinode.Izumi.Services.RpgServices.ProductService;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.ShopCommands.BuyCommands.Impl
{
    [InjectableService]
    public class ShopBuyProductCommand : IShopBuyProductCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IInventoryService _inventoryService;
        private readonly IImageService _imageService;
        private readonly ILocalizationService _local;
        private readonly IProductService _productService;

        public ShopBuyProductCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IInventoryService inventoryService, IImageService imageService, ILocalizationService local,
            IProductService productService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _inventoryService = inventoryService;
            _imageService = imageService;
            _local = local;
            _productService = productService;
        }

        public async Task Execute(SocketCommandContext context, long productId, long amount)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем продукт
            var product = await _productService.GetProduct(productId);
            // получаем валюту пользователя
            var userCurrency = await _inventoryService.GetUserCurrency((long) context.User.Id, Currency.Ien);

            // проверяем хватит ли пользователю денег на оплату продуктов
            if (userCurrency.Amount < product.Price * amount)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.ShopBuyNoCurrency.Parse(
                    emotes.GetEmoteOrBlank(Currency.Ien.ToString()), _local.Localize(Currency.Ien.ToString(), 5))));
            }
            else
            {
                // забираем у пользователя деньги на оплату продуктов
                await _inventoryService.RemoveItemFromUser(
                    (long) context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(),
                    product.Price * amount);
                // добавляем пользователю продукты
                await _inventoryService.AddItemToUser(
                    (long) context.User.Id, InventoryCategory.Product, product.Id, amount);

                var embed = new EmbedBuilder()
                    // баннер магазина продуктов
                    .WithImageUrl(await _imageService.GetImageUrl(Image.ShopProduct))
                    // подверждаем успешную покупку продуктов
                    .WithDescription(IzumiReplyMessage.ProductShopBuySuccess.Parse(
                        emotes.GetEmoteOrBlank(product.Name), amount, _local.Localize(product.Name, amount),
                        emotes.GetEmoteOrBlank(Currency.Ien.ToString()), product.Price * amount,
                        _local.Localize(Currency.Ien.ToString(), product.Price * amount)));

                await _discordEmbedService.SendEmbed(context.User, embed);
                await Task.CompletedTask;
            }
        }
    }
}
