using MediatR;

namespace Hinode.Izumi.Services.GameServices.UserService.Commands
{
    public record RemoveEnergyFromUserCommand(long Id, long Amount) : IRequest;
}
