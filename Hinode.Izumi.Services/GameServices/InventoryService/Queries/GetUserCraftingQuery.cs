using Hinode.Izumi.Services.GameServices.InventoryService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Queries
{
    public record GetUserCraftingQuery(long UserId, long CraftingId) : IRequest<UserCraftingRecord>;
}
