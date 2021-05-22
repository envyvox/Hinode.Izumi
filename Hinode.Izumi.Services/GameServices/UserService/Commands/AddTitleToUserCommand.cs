using Hinode.Izumi.Data.Enums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.UserService.Commands
{
    public record AddTitleToUserCommand(long Id, Title Title) : IRequest;
}
