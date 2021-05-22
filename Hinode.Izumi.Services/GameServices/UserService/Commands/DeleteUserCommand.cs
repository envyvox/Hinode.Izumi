using MediatR;

namespace Hinode.Izumi.Services.GameServices.UserService.Commands
{
    public record DeleteUserCommand(long Id) : IRequest;
}
