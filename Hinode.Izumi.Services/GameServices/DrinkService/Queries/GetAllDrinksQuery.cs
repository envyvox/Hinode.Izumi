using Hinode.Izumi.Services.GameServices.DrinkService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.DrinkService.Queries
{
    public record GetAllDrinksQuery : IRequest<DrinkRecord[]>;
}
