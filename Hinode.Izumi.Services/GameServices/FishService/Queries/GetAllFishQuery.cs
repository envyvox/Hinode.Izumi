using Hinode.Izumi.Services.GameServices.FishService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FishService.Queries
{
    public record GetAllFishQuery : IRequest<FishRecord[]>;
}
