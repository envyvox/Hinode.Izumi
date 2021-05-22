using MediatR;

namespace Hinode.Izumi.Services.GameServices.IngredientService.Queries
{
    public record DisplayDrinkIngredientsQuery(long DrinkId, long Amount = 1) : IRequest<string>;
}
