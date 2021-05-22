using Hinode.Izumi.Data.Enums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Queries
{
    public record CheckItemInUserQuery(long UserId, InventoryCategory Category, long ItemId) : IRequest<bool>;
}
