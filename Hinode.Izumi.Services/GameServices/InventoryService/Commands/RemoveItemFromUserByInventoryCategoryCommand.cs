using Hinode.Izumi.Data.Enums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Commands
{
    public record RemoveItemFromUserByInventoryCategoryCommand(
            long UserId,
            InventoryCategory Category,
            long ItemId,
            long Amount = 1)
        : IRequest;
}
