using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using Hinode.Izumi.Services.GameServices.InventoryService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.MasteryService.Queries;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using Hinode.Izumi.Services.GameServices.SeedService.Queries;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.ShopCommands.BuyCommands.Impl
{
    [InjectableService]
    public class ShopBuySeedCommand : IShopBuySeedCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public ShopBuySeedCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context, long seedId, long amount)
        {
            // получаем желаемое семя
            var seed = await _mediator.Send(new GetSeedQuery(seedId));
            // получаем текущий сезон
            var season = (Season) await _mediator.Send(new GetPropertyValueQuery(Property.CurrentSeason));

            // проверяем подходит ли сезон семени для покупки
            if (seed.Season != season)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.ShopBuySeedWrongSeason.Parse(
                    seed.Season.Localize())));
            }
            else
            {
                // получаем иконки из базы
                var emotes = await _mediator.Send(new GetEmotesQuery());
                // получаем валюту пользователя
                var userCurrency = await _mediator.Send(new GetUserCurrencyQuery(
                    (long) context.User.Id, Currency.Ien));
                // получаем мастерсто торговли пользователя
                var userMastery = await _mediator.Send(new GetUserMasteryQuery(
                    (long) context.User.Id, Mastery.Trading));
                // определяем стоимость семени после скидки мастерства торговли
                var seedPrice = await _mediator.Send(new GetSeedPriceWithDiscountQuery(
                    // округяем мастерство пользователя
                    (long) Math.Floor(userMastery.Amount), seed.Price));

                // проверяем хватает ли пользователю денег на оплату семян
                if (userCurrency.Amount < seedPrice * amount)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.ShopBuyNoCurrency.Parse(
                        emotes.GetEmoteOrBlank(Currency.Ien.ToString()),
                        _local.Localize(Currency.Ien.ToString(), 5))));
                }
                else
                {
                    // забираем у пользователя деньги на оплату семян
                    await _mediator.Send(new RemoveItemFromUserByInventoryCategoryCommand(
                        (long) context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(),
                        seedPrice * amount));
                    // добавляем пользователю семена
                    await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                        (long) context.User.Id, InventoryCategory.Seed, seed.Id, amount));

                    var embed = new EmbedBuilder()
                        // баннер магазина семян
                        .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.ShopSeed)))
                        // подверждаем успешную покупку семян
                        .WithDescription(IzumiReplyMessage.ShopBuySeedSuccess.Parse(
                            emotes.GetEmoteOrBlank(seed.Name), amount, _local.Localize(seed.Name, amount),
                            emotes.GetEmoteOrBlank(Currency.Ien.ToString()), seedPrice * amount,
                            _local.Localize(Currency.Ien.ToString(), seedPrice * amount)));

                    await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
                    await Task.CompletedTask;
                }
            }
        }
    }
}
