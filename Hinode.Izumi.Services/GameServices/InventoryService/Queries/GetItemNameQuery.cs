using Hinode.Izumi.Data.Enums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Queries
{
    public record GetItemNameQuery(InventoryCategory Category, long ItemId) : IRequest<string>;
}
