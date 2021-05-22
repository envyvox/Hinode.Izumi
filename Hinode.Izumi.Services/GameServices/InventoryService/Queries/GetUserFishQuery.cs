using Hinode.Izumi.Services.GameServices.InventoryService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Queries
{
    public record GetUserFishQuery(long UserId, long FishId) : IRequest<UserFishRecord>;
}
