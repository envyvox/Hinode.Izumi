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
using Hinode.Izumi.Services.RpgServices.FieldService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.PropertyService;
using Hinode.Izumi.Services.RpgServices.SeedService;
using Hinode.Izumi.Services.RpgServices.UserService;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.FieldCommands.FieldDigCommand
{
    [InjectableService]
    public class FieldDigCommand : IFieldDigCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IFieldService _fieldService;
        private readonly IUserService _userService;
        private readonly IPropertyService _propertyService;
        private readonly IImageService _imageService;
        private readonly ILocalizationService _local;
        private readonly ISeedService _seedService;

        public FieldDigCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IFieldService fieldService, IUserService userService, IPropertyService propertyService,
            IImageService imageService, ILocalizationService local, ISeedService seedService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _fieldService = fieldService;
            _userService = userService;
            _propertyService = propertyService;
            _imageService = imageService;
            _local = local;
            _seedService = seedService;
        }

        public async Task Execute(SocketCommandContext context, long fieldId)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем клетку участка пользователя
            var userField = await _fieldService.GetUserField((long) context.User.Id, fieldId);

            // проверяем не пустая ли она
            if (userField.State == FieldState.Empty)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.UserFieldDigEmpty.Parse()));
            }
            else
            {
                // сбрасываем состояние клетки участка на пустое
                await _fieldService.ResetField((long) context.User.Id, fieldId);
                // отнимаем энергию у пользователя
                await _userService.RemoveEnergyFromUser((long) context.User.Id,
                    // получаем количество энергии
                    await _propertyService.GetPropertyValue(Property.EnergyCostFieldDig));

                // получаем посажанные в этой клетке участка семена
                var seed = await _seedService.GetSeed(userField.SeedId);

                var embed = new EmbedBuilder()
                    // баннер участка
                    .WithImageUrl(await _imageService.GetImageUrl(Image.Field))
                    // подтверждаем что семена выкопаны с клетки участка
                    .WithDescription(IzumiReplyMessage.UserFieldDigSuccess.Parse(
                        emotes.GetEmoteOrBlank(seed.Name), _local.Localize(seed.Name)));

                await _discordEmbedService.SendEmbed(context.User, embed);
                await Task.CompletedTask;
            }
        }
    }
}
