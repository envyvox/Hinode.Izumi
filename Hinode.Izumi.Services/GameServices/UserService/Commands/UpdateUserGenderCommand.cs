using Hinode.Izumi.Data.Enums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.UserService.Commands
{
    public record UpdateUserGenderCommand(long Id, Gender Gender) : IRequest;
}
