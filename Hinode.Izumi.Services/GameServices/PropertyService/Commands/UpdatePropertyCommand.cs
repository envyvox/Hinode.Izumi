using Hinode.Izumi.Data.Enums.PropertyEnums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.PropertyService.Commands
{
    public record UpdatePropertyCommand(Property Property, long NewValue) : IRequest;
}
