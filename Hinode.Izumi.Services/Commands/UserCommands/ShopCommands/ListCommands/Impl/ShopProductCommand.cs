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
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.ProductService;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.ShopCommands.ListCommands.Impl
{
    [InjectableService]
    public class ShopProductCommand : IShopProductCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IImageService _imageService;
        private readonly IProductService _productService;
        private readonly ILocalizationService _local;

        public ShopProductCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IImageService imageService, IProductService productService, ILocalizationService local)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _imageService = imageService;
            _productService = productService;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем продукты из базы
            var products = await _productService.GetAllProducts();

            var embed = new EmbedBuilder()
                // рассказываем как покупать
                .WithDescription(
                    IzumiReplyMessage.ProductShopDesc.Parse() +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // баннер магазина продуктов
                .WithImageUrl(await _imageService.GetImageUrl(Image.ShopProduct));

            // для каждого продукта создаем embed field
            foreach (var product in products)
            {
                embed.AddField(IzumiReplyMessage.ProductShopFieldName.Parse(
                        emotes.GetEmoteOrBlank("List"), product.Id, emotes.GetEmoteOrBlank(product.Name),
                        _local.Localize(product.Name), emotes.GetEmoteOrBlank(Currency.Ien.ToString()), product.Price,
                        _local.Localize(Currency.Ien.ToString(), product.Price)),
                    emotes.GetEmoteOrBlank("Blank"));
            }

            await _discordEmbedService.SendEmbed(context.User, embed);
            await Task.CompletedTask;
        }
    }
}
