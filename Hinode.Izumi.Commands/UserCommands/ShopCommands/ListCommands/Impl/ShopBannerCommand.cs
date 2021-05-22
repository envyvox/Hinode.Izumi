using System;
using System.Globalization;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.RarityEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.BannerService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.ImageService.Queries;
using Humanizer;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.ShopCommands.ListCommands.Impl
{
    [InjectableService]
    public class ShopBannerCommand : IShopBannerCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public ShopBannerCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем баннеры из сменяемого магазина
            var banners = await _mediator.Send(new GetDynamicShopBannersQuery());

            var embed = new EmbedBuilder()
                // баннер магазина баннеров
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.ShopBanner)))
                .WithDescription(
                    // рассказываем как покупать баннеры
                    IzumiReplyMessage.ShopBannerDesc.Parse() +
                    // рассказываем о том, что товары меняются каждый день
                    IzumiReplyMessage.DynamicShopDesc.Parse() +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // выводим сколько осталось времени до смены товаров
                .WithFooter(IzumiReplyMessage.DynamicShopFooter.Parse(
                    (DateTime.Now - DateTime.Today.AddDays(1)).Humanize(2, new CultureInfo("ru-RU"))));

            // создаем embed field с информацией о каждом баннере
            foreach (var banner in banners)
            {
                embed.AddField(
                    $"{emotes.GetEmoteOrBlank("List")} `{banner.Id}` {banner.Rarity.Localize()} «{banner.Name}»",
                    IzumiReplyMessage.ShopBannerFieldDesc.Parse(
                        emotes.GetEmoteOrBlank(Currency.Ien.ToString()), banner.Price,
                        _local.Localize(Currency.Ien.ToString(), banner.Price), banner.Url));
            }

            await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
            await Task.CompletedTask;
        }
    }
}
