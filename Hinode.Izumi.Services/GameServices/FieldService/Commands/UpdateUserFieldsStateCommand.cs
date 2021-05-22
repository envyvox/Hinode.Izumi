using Hinode.Izumi.Data.Enums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FieldService.Commands
{
    public record UpdateUserFieldsStateCommand(long UserId, FieldState State) : IRequest;
}
