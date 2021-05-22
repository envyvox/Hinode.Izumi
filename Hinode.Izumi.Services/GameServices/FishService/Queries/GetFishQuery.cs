using Hinode.Izumi.Services.GameServices.FishService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FishService.Queries
{
    public record GetFishQuery(long Id) : IRequest<FishRecord>;
}
