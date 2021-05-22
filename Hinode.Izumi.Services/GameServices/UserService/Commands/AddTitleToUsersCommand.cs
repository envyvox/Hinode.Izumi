using Hinode.Izumi.Data.Enums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.UserService.Commands
{
    public record AddTitleToUsersCommand(long[] UsersId, Title Title) : IRequest;
}
