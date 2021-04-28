using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.BuildingService;
using Hinode.Izumi.Services.RpgServices.FieldService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.InventoryService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.PropertyService;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.FieldCommands.FieldBuyCommand
{
    [InjectableService]
    public class FieldBuyCommand : IFieldBuyCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IFieldService _fieldService;
        private readonly IInventoryService _inventoryService;
        private readonly IImageService _imageService;
        private readonly IPropertyService _propertyService;
        private readonly ILocalizationService _local;
        private readonly IBuildingService _buildingService;

        public FieldBuyCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IFieldService fieldService, IInventoryService inventoryService, IImageService imageService,
            IPropertyService propertyService, ILocalizationService local, IBuildingService buildingService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _fieldService = fieldService;
            _inventoryService = inventoryService;
            _imageService = imageService;
            _propertyService = propertyService;
            _local = local;
            _buildingService = buildingService;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем клетки участка пользователя
            var userFields = await _fieldService.GetUserField((long) context.User.Id);
            // получаем валюту пользователя
            var userCurrency = await _inventoryService.GetUserCurrency((long) context.User.Id, Currency.Ien);
            // получаем стоимость участка земли
            var fieldPrice = await _propertyService.GetPropertyValue(Property.FieldPrice);

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
                await _buildingService.AddBuildingToUser((long) context.User.Id, Building.HarvestField);
                // добавляем пользователю клетки участка
                await _fieldService.AddFieldToUser((long) context.User.Id, new long[] {1, 2, 3, 4, 5});
                // забираем у пользователя валюту за оплату участка
                await _inventoryService.RemoveItemFromUser(
                    (long) context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(), fieldPrice);

                var embed = new EmbedBuilder()
                    // баннер участка
                    .WithImageUrl(await _imageService.GetImageUrl(Image.Field))
                    // подверждаем успешную покупку участка
                    .WithDescription(IzumiReplyMessage.FieldBuySuccess.Parse(
                        emotes.GetEmoteOrBlank(Currency.Ien.ToString()), fieldPrice,
                        _local.Localize(Currency.Ien.ToString(), fieldPrice)));

                await _discordEmbedService.SendEmbed(context.User, embed);
                await Task.CompletedTask;
            }
        }
    }
}
