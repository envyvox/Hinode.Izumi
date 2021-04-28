using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.FamilyEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.FamilyService;
using Hinode.Izumi.Services.RpgServices.InventoryService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;

namespace Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.CurrencyCommands.FamilyCurrencyAddCommand
{
    [InjectableService]
    public class FamilyCurrencyAddCommand : IFamilyCurrencyAddCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly ILocalizationService _local;
        private readonly IFamilyService _familyService;
        private readonly IInventoryService _inventoryService;

        public FamilyCurrencyAddCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            ILocalizationService local, IFamilyService familyService, IInventoryService inventoryService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _local = local;
            _familyService = familyService;
            _inventoryService = inventoryService;
        }

        public async Task Execute(SocketCommandContext context, long amount, string currencyNamePattern)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем пользователя в семье
            var userFamily = await _familyService.GetUserFamily((long) context.User.Id);
            // получаем семью пользователя
            var family = await _familyService.GetFamily(userFamily.FamilyId);

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
                var userCurrency = await _inventoryService.GetUserCurrency(
                    (long) context.User.Id, (Currency) currencyLocal.ItemId);

                // проверяем у пользователя достаточно валюты чтобы добавить ее в казну семьи
                if (userCurrency.Amount < amount)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.FamilyCurrencyAddUserNoCurrency.Parse(
                        emotes.GetEmoteOrBlank(currencyLocal.Name), _local.Localize(currencyLocal.Name), 5)));
                }
                else
                {
                    // забираем у пользователя валюту
                    await _inventoryService.RemoveItemFromUser(
                        (long) context.User.Id, InventoryCategory.Currency, currencyLocal.ItemId, amount);
                    // добавляем семье валюту
                    await _familyService.AddCurrencyToFamily(family.Id, (Currency) currencyLocal.ItemId, amount);

                    var embed = new EmbedBuilder()
                        // подверждаем что валюта успешно добавлена в казну
                        .WithDescription(IzumiReplyMessage.FamilyCurrencyAddSuccess.Parse(
                            emotes.GetEmoteOrBlank(currencyLocal.Name), amount,
                            _local.Localize(currencyLocal.Name), amount));

                    await _discordEmbedService.SendEmbed(context.User, embed);
                    await Task.CompletedTask;
                }
            }
        }
    }
}
