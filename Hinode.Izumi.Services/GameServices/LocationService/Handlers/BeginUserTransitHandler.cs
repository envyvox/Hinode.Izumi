using System;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Services.BackgroundJobs.TransitJob;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Commands;
using Hinode.Izumi.Services.GameServices.LocationService.Commands;
using Hinode.Izumi.Services.GameServices.LocationService.Queries;
using Hinode.Izumi.Services.HangfireJobService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.LocationService.Handlers
{
    public class BeginUserTransitHandler : IRequestHandler<BeginUserTransitCommand>
    {
        private readonly IMediator _mediator;

        public BeginUserTransitHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(BeginUserTransitCommand request, CancellationToken cancellationToken)
        {
            var (userId, departure, destination, time) = request;
            var arrival = DateTimeOffset.Now.AddMinutes(time);

            // обновляем текущую локацию пользователя
            await _mediator.Send(new UpdateUserLocationCommand(userId, Location.InTransit), cancellationToken);
            // добавляем информацию о перемещении
            await _mediator.Send(new CreateUserMovementCommand(userId, departure, destination, arrival),
                cancellationToken);
            // снимаем роль текущей локации
            await _mediator.Send(new RemoveDiscordRoleFromUserCommand(userId,
                await _mediator.Send(new GetLocationRoleQuery(departure), cancellationToken)), cancellationToken);
            // добавляем роль перемещения
            await _mediator.Send(new AddDiscordRoleToUserCommand(userId, DiscordRole.LocationInTransit),
                cancellationToken);

            var jobId = BackgroundJob.Schedule<ITransitJob>(x =>
                    x.CompleteTransit(userId, destination),
                TimeSpan.FromMinutes(time));
            await _mediator.Send(new CreateUserHangfireJobCommand(
                    userId, HangfireAction.Transit, jobId), cancellationToken);

            return new Unit();
        }
    }
}
