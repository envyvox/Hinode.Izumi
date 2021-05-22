using MediatR;

namespace Hinode.Izumi.Services.GameServices.LocationService.Commands
{
    public record DeleteUserMovementCommand(long UserId) : IRequest;
}
