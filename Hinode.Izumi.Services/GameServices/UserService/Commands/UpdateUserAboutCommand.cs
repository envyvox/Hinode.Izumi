using MediatR;

namespace Hinode.Izumi.Services.GameServices.UserService.Commands
{
    public record UpdateUserAboutCommand(long Id, string NewAbout) : IRequest;
}
