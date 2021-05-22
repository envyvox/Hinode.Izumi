using Hinode.Izumi.Data.Enums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FieldService.Commands
{
    public record UpdateAllFieldsStateCommand(FieldState State) : IRequest;
}
