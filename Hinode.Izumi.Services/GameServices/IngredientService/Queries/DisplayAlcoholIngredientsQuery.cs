using MediatR;

namespace Hinode.Izumi.Services.GameServices.IngredientService.Queries
{
    public record DisplayAlcoholIngredientsQuery(long AlcoholId, long Amount = 1) : IRequest<string>;
}
