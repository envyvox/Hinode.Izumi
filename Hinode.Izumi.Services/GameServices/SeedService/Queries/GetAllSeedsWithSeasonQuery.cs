using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.GameServices.SeedService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.SeedService.Queries
{
    public record GetAllSeedsWithSeasonQuery(Season Season) : IRequest<SeedRecord[]>;
}
