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
using Hinode.Izumi.Services.GameServices.LocalizationService;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands.FamilyCommands.CurrencyCommands.FamilyCurrencyTakeCommand
{
    [InjectableService]
    public class FamilyCurrencyTakeCommand : IFamilyCurrencyTakeCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public FamilyCurrencyTakeCommand(IMediator mediator, ILocalizationService local)
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
            // проверяем что пользователь глава семьи или его заместитель
            else if (userFamily.Status != UserInFamilyStatus.Head &&
                     userFamily.Status != UserInFamilyStatus.Deputy)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.UserFamilyStatusRequireNotDefault.Parse()));
            }
            else
            {
                // получаем локализацию валюты
                var currencyLocal = await _local.GetLocalizationByLocalizedWord(
                    LocalizationCategory.Currency, currencyNamePattern);
                // получаем валюту семьи
                var familyCurrency = await _mediator.Send(new GetFamilyCurrencyQuery(
                    family.Id, (Currency) currencyLocal.ItemId));

                // проверяем что в казне семьи есть столько валюту
                if (familyCurrency.Amount < amount)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.FamilyCurrencyTakeFamilyNoCurrency.Parse(
                        emotes.GetEmoteOrBlank(currencyLocal.Name), _local.Localize(currencyLocal.Name), 5)));
                }
                else
                {
                    // забираем у семьи валюту
                    await _mediator.Send(new RemoveCurrencyFromFamilyCommand(
                        family.Id, (Currency) currencyLocal.ItemId, amount));
                    // добавляем пользователю валюту
                    await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                        (long) context.User.Id, InventoryCategory.Currency, currencyLocal.ItemId, amount));

                    var embed = new EmbedBuilder()
                        // подтверждаем что валюта успешно взята из казны
                        .WithDescription(IzumiReplyMessage.FamilyCurrencyTakeSuccess.Parse(
                            emotes.GetEmoteOrBlank(currencyLocal.Name), amount,
                            _local.Localize(currencyLocal.Name), amount));

                    await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
                    await Task.CompletedTask;
                }
            }
        }
    }
}
