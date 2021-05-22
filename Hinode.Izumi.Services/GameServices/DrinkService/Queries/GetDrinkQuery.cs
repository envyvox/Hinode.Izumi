using Hinode.Izumi.Services.GameServices.DrinkService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.DrinkService.Queries
{
    public record GetDrinkQuery(long Id) : IRequest<DrinkRecord>;
}
