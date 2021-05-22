using System;
using System.Collections;
using System.Linq;
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
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using Hinode.Izumi.Services.GameServices.InventoryService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Records;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.ShopCommands.FisherCommands.FisherSellCommand
{
    [InjectableService]
    public class FisherSellCommand : IFisherSellCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public FisherSellCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task SellFishWithIdAndAmount(SocketCommandContext context, long fishId, long amount)
        {
            // получаем рыбу пользователя
            var userFish = await _mediator.Send(new GetUserFishQuery((long) context.User.Id, fishId));
            // пробуем продать рыбу
            await SellFish(context, userFish, amount);
        }

        public async Task SellAllFishWithId(SocketCommandContext context, long fishId, string input)
        {
            if (!(input.Contains("все") || input.Contains("всю"))) return;

            // получаем рыбу пользователя
            var userFish = await _mediator.Send(new GetUserFishQuery((long) context.User.Id, fishId));
            // пробуем продать рыбу
            await SellFish(context, userFish, userFish.Amount);
        }

        public async Task SellAllFish(SocketCommandContext context, string input)
        {
            if (!(input.Contains("все") || input.Contains("всю"))) return;

            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем текущий сезон
            var season = (Season) await _mediator.Send(new GetPropertyValueQuery(Property.CurrentSeason));
            // получаем рыбу пользователя
            var userFish = await _mediator.Send(new GetUserFishesQuery((long) context.User.Id));
            // выбираем только ту, которая подходит по сезону
            var userSeasonFish = userFish
                .Where(x => x.Seasons.Contains(season) || x.Seasons.Contains(Season.Any))
                .ToArray();

            var soldFish = string.Empty;
            long totalCurrency = 0;

            // продаем каждую подходяющую рыбу по отдельности и добавляем ее в список
            foreach (var fish in userSeasonFish)
            {
                // проверяем что у пользователя не 0 этой рыбы
                if (fish.Amount < 1) continue;

                // отнимаем рыбу у пользователя
                await _mediator.Send(new RemoveItemFromUserByInventoryCategoryCommand(
                    (long) context.User.Id, InventoryCategory.Fish, fish.FishId, fish.Amount));
                // добавляем пользователю валюту за рыбу
                await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                    (long) context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(),
                    fish.Price * fish.Amount));

                // добавляем в общий список информацию о проданной рыбе
                totalCurrency += fish.Price * fish.Amount;
                soldFish += IzumiReplyMessage.FisherMassSellFishLine.Parse(
                    emotes.GetEmoteOrBlank(fish.Name), fish.Amount, _local.Localize(fish.Name, fish.Amount),
                    emotes.GetEmoteOrBlank(Currency.Ien.ToString()), fish.Price * fish.Amount,
                    _local.Localize(Currency.Ien.ToString(), fish.Price * fish.Amount));
            }

            // проверяем продалась ли хоть одна рыба
            if (totalCurrency < 1)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.FisherMassSellNoFish.Parse()));
            }
            else
            {
                // собираем итоговую информацию о проданной рыбе
                var soldFishString = IzumiReplyMessage.FisherMassSellSuccessFieldDesc.Parse(
                    soldFish, emotes.GetEmoteOrBlank(Currency.Ien.ToString()), totalCurrency,
                    _local.Localize(Currency.Ien.ToString(), totalCurrency));

                // если информация слишком длинная, выводим сокращенную версию
                if (soldFishString.Length > 1024)
                    soldFishString = IzumiReplyMessage.FisherMassSellSuccessFieldDesc.Parse(
                        IzumiReplyMessage.FisherMassSellFishLineOutOfLimit.Parse(),
                        emotes.GetEmoteOrBlank(Currency.Ien.ToString()), totalCurrency,
                        _local.Localize(Currency.Ien.ToString(), totalCurrency));

                var embed = new EmbedBuilder()
                    // подтверждаем успешную продажу рыбы
                    .WithDescription(IzumiReplyMessage.FisherMassSellSuccessDesc.Parse())
                    // баннер магазина рыбака
                    .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.ShopFisher)))
                    .AddField(IzumiReplyMessage.FisherMassSellSuccessFieldName.Parse(), soldFishString);

                await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
                await Task.CompletedTask;
            }
        }

        private async Task SellFish(SocketCommandContext context, UserFishRecord userFish, long amount)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем текущий сезон
            var season = (Season) await _mediator.Send(new GetPropertyValueQuery(Property.CurrentSeason));

            // проверяем не пытается ли пользователь продать больше, чем у него есть
            if (amount > userFish.Amount || amount == 0)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.FisherSellNoFish.Parse(
                    emotes.GetEmoteOrBlank(userFish.Name), _local.Localize(userFish.Name))));
            }
            // проверяем не пытается ли пользователь продать рыбу не подходящую по сезону
            else if (!((IList) userFish.Seasons).Contains(season) && !((IList) userFish.Seasons).Contains(Season.Any))
            {
                await Task.FromException(new Exception(IzumiReplyMessage.FisherSellWrongSeason.Parse()));
            }
            else
            {
                // отнимаем у пользователя рыбу
                await _mediator.Send(new RemoveItemFromUserByInventoryCategoryCommand(
                    (long) context.User.Id, InventoryCategory.Fish, userFish.FishId, amount));
                // добавляем пользователю валюту за рыбу
                await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                    (long) context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(),
                    userFish.Price * amount));

                var embed = new EmbedBuilder()
                    // подтверждаем успешную продажу
                    .WithDescription(IzumiReplyMessage.FisherSellSuccess.Parse(
                        emotes.GetEmoteOrBlank(userFish.Name), amount, _local.Localize(userFish.Name, amount),
                        emotes.GetEmoteOrBlank(Currency.Ien.ToString()), userFish.Price * amount,
                        _local.Localize(Currency.Ien.ToString(), userFish.Price * amount)))
                    // баннер магазина рыбака
                    .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.ShopFisher)));

                await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
                await Task.CompletedTask;
            }
        }
    }
}
