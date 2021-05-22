using MediatR;

namespace Hinode.Izumi.Services.GameServices.UserService.Commands
{
    public record AddEnergyToUsersCommand(long[] UsersId, int Amount) : IRequest;
}
