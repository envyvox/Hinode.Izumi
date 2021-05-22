using MediatR;

namespace Hinode.Izumi.Services.GameServices.ProjectService.Commands
{
    public record AddProjectToUserCommand(long UserId, long ProjectId) : IRequest;
}
