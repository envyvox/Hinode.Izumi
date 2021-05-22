using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.GameServices.PropertyService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.PropertyService.Queries
{
    public record GetCraftingPropertiesQuery(
            long CraftingId,
            CraftingProperty Property)
        : IRequest<CraftingPropertyRecord>;
}
