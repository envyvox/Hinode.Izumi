using Hinode.Izumi.Data.Enums.PropertyEnums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.PropertyService.Queries
{
    public record GetPropertyValueQuery(Property Property) : IRequest<long>;
}
