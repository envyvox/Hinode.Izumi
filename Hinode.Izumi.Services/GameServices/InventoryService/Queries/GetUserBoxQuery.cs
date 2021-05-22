using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.GameServices.InventoryService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Queries
{
    public record GetUserBoxQuery(long UserId, Box Box) : IRequest<UserBoxRecord>;
}
