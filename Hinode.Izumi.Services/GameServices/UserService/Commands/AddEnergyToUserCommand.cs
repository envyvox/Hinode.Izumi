using MediatR;

namespace Hinode.Izumi.Services.GameServices.UserService.Commands
{
    public record AddEnergyToUserCommand(long Id, long Amount) : IRequest;
}
