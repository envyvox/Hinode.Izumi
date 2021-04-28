using System.Threading.Tasks;
using Discord;
using Hangfire;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;
using Hinode.Izumi.Services.RpgServices.FieldService;
using Hinode.Izumi.Services.RpgServices.LocationService;

namespace Hinode.Izumi.Services.BackgroundJobs.FieldJob
{
    [InjectableService]
    public class FieldJob : IFieldJob
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly ILocationService _locationService;
        private readonly IDiscordGuildService _discordGuildService;
        private readonly IFieldService _fieldService;

        public FieldJob(IDiscordEmbedService discordEmbedService, ILocationService locationService,
            IDiscordGuildService discordGuildService, IFieldService fieldService)
        {
            _discordEmbedService = discordEmbedService;
            _locationService = locationService;
            _discordGuildService = discordGuildService;
            _fieldService = fieldService;
        }

        public async Task CompleteWatering(long userId, long fieldOwnerId)
        {
            // обновляем текущую локацию пользователя
            await _locationService.UpdateUserLocation(userId, Location.Village);
            // удаляем информацию о перемещении
            await _locationService.RemoveUserMovement(userId);
            // обновляем состояние ячеек участка на политые
            await _fieldService.UpdateState(fieldOwnerId, FieldState.Watered);

            var embed = new EmbedBuilder()
                // оповещаем о завершении поливки участка
                .WithDescription(IzumiReplyMessage.UserFieldWaterSuccess.Parse());

            await _discordEmbedService.SendEmbed(
                await _discordGuildService.GetSocketUser(userId), embed);
        }
    }
}
