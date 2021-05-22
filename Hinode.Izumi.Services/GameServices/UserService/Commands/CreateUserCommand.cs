using MediatR;

namespace Hinode.Izumi.Services.GameServices.UserService.Commands
{
    public record CreateUserCommand(long Id, string Name) : IRequest;
}
