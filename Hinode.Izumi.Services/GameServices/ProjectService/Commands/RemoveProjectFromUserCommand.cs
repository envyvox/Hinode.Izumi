using MediatR;

namespace Hinode.Izumi.Services.GameServices.ProjectService.Commands
{
    public record RemoveProjectFromUserCommand(long UserId, long ProjectId) : IRequest;
}
