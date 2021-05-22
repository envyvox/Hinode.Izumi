using Hinode.Izumi.Services.GameServices.AlcoholService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.AlcoholService.Queries
{
    public record GetAllAlcoholQuery : IRequest<AlcoholRecord[]>;
}
