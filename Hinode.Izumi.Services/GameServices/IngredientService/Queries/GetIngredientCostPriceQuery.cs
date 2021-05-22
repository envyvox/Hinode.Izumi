using Hinode.Izumi.Data.Enums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.IngredientService.Queries
{
    public record GetIngredientCostPriceQuery(IngredientCategory Category, long IngredientId) : IRequest<long>;
}
