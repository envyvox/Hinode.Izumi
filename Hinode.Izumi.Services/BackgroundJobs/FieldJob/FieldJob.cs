using System.Threading.Tasks;
using Discord;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using Hinode.Izumi.Services.GameServices.FieldService.Commands;
using Hinode.Izumi.Services.GameServices.LocationService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.BackgroundJobs.FieldJob
{
    [InjectableService]
    public class FieldJob : IFieldJob
    {
        private readonly IMediator _mediator;

        public FieldJob(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task CompleteWatering(long userId, long fieldOwnerId)
        {
            // обновляем текущую локацию пользователя
            await _mediator.Send(new UpdateUserLocationCommand(userId, Location.Village));
            // удаляем информацию о перемещении
            await _mediator.Send(new DeleteUserMovementCommand(userId));
            // обновляем состояние ячеек участка на политые
            await _mediator.Send(new UpdateUserFieldsStateCommand(fieldOwnerId, FieldState.Watered));

            var embed = new EmbedBuilder()
                // оповещаем о завершении поливки участка
                .WithDescription(IzumiReplyMessage.UserFieldWaterSuccess.Parse());

            await _mediator.Send(new SendEmbedToUserCommand(
                await _mediator.Send(new GetDiscordSocketUserQuery(userId)), embed));
        }
    }
}
