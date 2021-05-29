using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.FamilyEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.FamilyService.Commands;
using Hinode.Izumi.Services.GameServices.FamilyService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using Hinode.Izumi.Services.GameServices.InventoryService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands.FamilyCommands.CurrencyCommands.FamilyCurrencyAddCommand
{
    [InjectableService]
    public class FamilyCurrencyAddCommand : IFamilyCurrencyAddCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public FamilyCurrencyAddCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context, long amount, string currencyNamePattern)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем пользователя в семье
            var userFamily = await _mediator.Send(new GetUserFamilyQuery((long) context.User.Id));
            // получаем семью пользователя
            var family = await _mediator.Send(new GetFamilyByIdQuery(userFamily.FamilyId));

            // проверяем что семья прошла этап регистрации
            if (family.Status == FamilyStatus.Registration)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.FamilyStatusRegistration.Parse()));
            }
            else
            {
                // получаем локализацию валюты
                var currencyLocal = await _local.GetLocalizationByLocalizedWord(
                    LocalizationCategory.Currency, currencyNamePattern);
                // получаем валюту пользователя
                var userCurrency = await _mediator.Send(new GetUserCurrencyQuery(
                    (long) context.User.Id, (Currency) currencyLocal.ItemId));

                // проверяем у пользователя достаточно валюты чтобы добавить ее в казну семьи
                if (userCurrency.Amount < amount)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.FamilyCurrencyAddUserNoCurrency.Parse(
                        emotes.GetEmoteOrBlank(currencyLocal.Name), _local.Localize(currencyLocal.Name), 5)));
                }
                else
                {
                    // забираем у пользователя валюту
                    await _mediator.Send(new RemoveItemFromUserByInventoryCategoryCommand(
                        (long) context.User.Id, InventoryCategory.Currency, currencyLocal.ItemId, amount));
                    // добавляем семье валюту
                    await _mediator.Send(new AddCurrencyToFamilyCommand(
                        family.Id, (Currency) currencyLocal.ItemId, amount));

                    var embed = new EmbedBuilder()
                        // подверждаем что валюта успешно добавлена в казну
                        .WithDescription(IzumiReplyMessage.FamilyCurrencyAddSuccess.Parse(
                            emotes.GetEmoteOrBlank(currencyLocal.Name), amount,
                            _local.Localize(currencyLocal.Name), amount));

                    await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
                    await Task.CompletedTask;
                }
            }
        }
    }
}
