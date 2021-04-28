using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hangfire;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.BackgroundJobs.FieldJob;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.RpgServices.CalculationService;
using Hinode.Izumi.Services.RpgServices.FamilyService;
using Hinode.Izumi.Services.RpgServices.FieldService;
using Hinode.Izumi.Services.RpgServices.FieldService.Models;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.LocationService;
using Hinode.Izumi.Services.RpgServices.PropertyService;
using Hinode.Izumi.Services.RpgServices.UserService;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.FieldCommands.FieldWaterCommand
{
    [InjectableService]
    public class FieldWaterCommand : IFieldWaterCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IFieldService _fieldService;
        private readonly ILocationService _locationService;
        private readonly TimeZoneInfo _timeZoneInfo;
        private readonly IUserService _userService;
        private readonly IFamilyService _familyService;
        private readonly IImageService _imageService;
        private readonly IPropertyService _propertyService;
        private readonly ICalculationService _calc;

        public FieldWaterCommand(IDiscordEmbedService discordEmbedService, IFieldService fieldService,
            ILocationService locationService, TimeZoneInfo timeZoneInfo, IUserService userService,
            IFamilyService familyService, IImageService imageService, IPropertyService propertyService,
            ICalculationService calc)
        {
            _discordEmbedService = discordEmbedService;
            _fieldService = fieldService;
            _locationService = locationService;
            _timeZoneInfo = timeZoneInfo;
            _userService = userService;
            _familyService = familyService;
            _imageService = imageService;
            _propertyService = propertyService;
            _calc = calc;
        }

        public async Task Execute(SocketCommandContext context, string namePattern = null)
        {
            // если пользователь не ввел игровое имя в команде, значит он хочет полить свой участок
            if (namePattern == null)
            {
                // получаем клетки земли участка пользователя
                var userFields = await _fieldService.GetUserField((long) context.User.Id);
                // поливаем участок пользователя
                await WaterFieldTask(context, userFields);
            }
            else
            {
                // получаем цель пользователя
                var tUser = await _userService.GetUser(namePattern);
                // получаем семью пользователя
                var userFamily = await _familyService.GetUserFamily((long) context.User.Id);
                // получаем семью цели пользователя
                var tUserFamily = await _familyService.GetUserFamily(tUser.Id);

                // проверяем в одной ли они семье
                if (userFamily == null ||
                    tUserFamily == null ||
                    userFamily.FamilyId != tUserFamily.FamilyId)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.UserFieldWaterFamilyMemberOnly.Parse()));
                }
                else
                {
                    // получаем клетки земли участка пользователя
                    var userFields = await _fieldService.GetUserField(tUser.Id);
                    // поливаем участок цели пользователя
                    await WaterFieldTask(context, userFields);
                }
            }
        }

        private async Task WaterFieldTask(SocketCommandContext context, IReadOnlyCollection<UserFieldModel> userFields)
        {
            // проверяем есть ли у пользователя участок
            if (userFields.Count < 1)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.UserFieldNull.Parse()));
            }
            else
            {
                // считаем сколько клеток нуждаются в поливке
                var fieldToWater = userFields.Count(field => field.State == FieldState.Planted);

                // проверяем нужно ли вообще поливать участок
                if (fieldToWater < 1)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.UserFieldWaterNull.Parse()));
                }
                else
                {
                    // получаем пользователя который поливает участок
                    var user = await _userService.GetUser((long) context.User.Id);
                    // получаем текущее время
                    var timeNow = TimeZoneInfo.ConvertTime(DateTime.Now, _timeZoneInfo);
                    // определяем длительность поливки участка
                    var wateringTime = _calc.ActionTime(
                        await _propertyService.GetPropertyValue(Property.ActionTimeFieldWater), user.Energy);

                    // обновляем текущую локацию пользователя
                    await _locationService.UpdateUserLocation((long) context.User.Id, Location.FieldWatering);
                    // добавляем информацию о перемещении
                    await _locationService.AddUserMovement(
                        (long) context.User.Id, Location.FieldWatering, Location.Village,
                        timeNow.AddMinutes(wateringTime));
                    // отнимаем энергию у пользователя
                    await _userService.RemoveEnergyFromUser((long) context.User.Id,
                        // получаем количество энергии
                        await _propertyService.GetPropertyValue(Property.EnergyCostFieldWater));

                    // запускаем джобу для окончания поливки участка
                    BackgroundJob.Schedule<IFieldJob>(x =>
                            x.CompleteWatering((long) context.User.Id, userFields.First().UserId),
                        TimeSpan.FromMinutes(wateringTime));

                    var embed = new EmbedBuilder()
                        // баннер участка
                        .WithImageUrl(await _imageService.GetImageUrl(Image.Field))
                        // подверждаем успешное начало поливки участка
                        .WithDescription(IzumiReplyMessage.UserFieldWaterStart.Parse(wateringTime));

                    await _discordEmbedService.SendEmbed(context.User, embed);
                    await Task.CompletedTask;
                }
            }
        }
    }
}
