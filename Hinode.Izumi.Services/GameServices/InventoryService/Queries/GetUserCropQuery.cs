using Hinode.Izumi.Services.GameServices.InventoryService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Queries
{
    public record GetUserCropQuery(long UserId, long CropId) : IRequest<UserCropRecord>;
}
