using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.GameServices.FishService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FishService.Queries
{
    public record GetAllFishBySeasonQuery(Season Season) : IRequest<FishRecord[]>;
}
