using Hinode.Izumi.Services.GameServices.CraftingService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CraftingService.Queries
{
    public record GetCraftingQuery(long Id) : IRequest<CraftingRecord>;
}
