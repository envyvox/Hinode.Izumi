using Hinode.Izumi.Data.Enums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.UserService.Commands
{
    public record UpdateUserTitleCommand(long Id, Title Title) : IRequest;
}
