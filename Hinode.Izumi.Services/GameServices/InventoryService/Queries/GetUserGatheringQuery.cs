using Hinode.Izumi.Services.GameServices.InventoryService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Queries
{
    public record GetUserGatheringQuery(long UserId, long GatheringId) : IRequest<UserGatheringRecord>;
}
