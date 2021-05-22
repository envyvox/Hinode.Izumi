using Hinode.Izumi.Services.GameServices.SeedService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.SeedService.Queries
{
    public record GetSeedQuery(long Id) : IRequest<SeedRecord>;
}
