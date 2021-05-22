using MediatR;

namespace Hinode.Izumi.Services.GameServices.UserService.Commands
{
    public record UpdateUserNameCommand(long Id, string NewName) : IRequest;
}
