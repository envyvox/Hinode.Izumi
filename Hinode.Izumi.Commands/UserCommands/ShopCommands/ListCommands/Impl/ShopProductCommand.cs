using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.ProductService.Queries;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.ShopCommands.ListCommands.Impl
{
    [InjectableService]
    public class ShopProductCommand : IShopProductCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public ShopProductCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем продукты из базы
            var products = await _mediator.Send(new GetAllProductsQuery());

            var embed = new EmbedBuilder()
                // рассказываем как покупать
                .WithDescription(
                    IzumiReplyMessage.ProductShopDesc.Parse() +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // баннер магазина продуктов
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.ShopProduct)));

            // для каждого продукта создаем embed field
            foreach (var product in products)
            {
                embed.AddField(IzumiReplyMessage.ProductShopFieldName.Parse(
                        emotes.GetEmoteOrBlank("List"), product.Id, emotes.GetEmoteOrBlank(product.Name),
                        _local.Localize(product.Name), emotes.GetEmoteOrBlank(Currency.Ien.ToString()), product.Price,
                        _local.Localize(Currency.Ien.ToString(), product.Price)),
                    emotes.GetEmoteOrBlank("Blank"));
            }

            await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
            await Task.CompletedTask;
        }
    }
}
