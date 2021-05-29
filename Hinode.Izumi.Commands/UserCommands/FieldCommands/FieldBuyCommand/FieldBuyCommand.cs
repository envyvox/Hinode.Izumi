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
using Hinode.Izumi.Services.GameServices.BuildingService.Commands;
using Hinode.Izumi.Services.GameServices.FieldService.Commands;
using Hinode.Izumi.Services.GameServices.FieldService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using Hinode.Izumi.Services.GameServices.InventoryService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.FieldCommands.FieldBuyCommand
{
    [InjectableService]
    public class FieldBuyCommand : IFieldBuyCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public FieldBuyCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем клетки участка пользователя
            var userFields = await _mediator.Send(new GetUserFieldsQuery((long) context.User.Id));
            // получаем валюту пользователя
            var userCurrency = await _mediator.Send(new GetUserCurrencyQuery((long) context.User.Id, Currency.Ien));
            // получаем стоимость участка земли
            var fieldPrice = await _mediator.Send(new GetPropertyValueQuery(Property.FieldPrice));

            // если у пользователя есть клетки участка - ему не нужно ничего покупать
            if (userFields.Length > 0)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.FieldBuyAlready.Parse()));
            }
            // проверяем есть у пользователя деньги на оплату участка
            else if (userCurrency.Amount < fieldPrice)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.FieldBuyNoCurrency.Parse(
                    emotes.GetEmoteOrBlank(Currency.Ien.ToString()),
                    _local.Localize(Currency.Ien.ToString(), 5))));
            }
            else
            {
                // добавляем пользователю постройку участок
                await _mediator.Send(new AddBuildingToUserCommand((long) context.User.Id, Building.HarvestField));
                // добавляем пользователю клетки участка
                await _mediator.Send(new CreateUserFieldsCommand((long) context.User.Id, new long[] {1, 2, 3, 4, 5}));
                // забираем у пользователя валюту за оплату участка
                await _mediator.Send(new RemoveItemFromUserByInventoryCategoryCommand(
                    (long) context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(), fieldPrice));

                var embed = new EmbedBuilder()
                    // баннер участка
                    .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.Field)))
                    // подверждаем успешную покупку участка
                    .WithDescription(IzumiReplyMessage.FieldBuySuccess.Parse(
                        emotes.GetEmoteOrBlank(Currency.Ien.ToString()), fieldPrice,
                        _local.Localize(Currency.Ien.ToString(), fieldPrice)));

                await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
                await Task.CompletedTask;
            }
        }
    }
}
