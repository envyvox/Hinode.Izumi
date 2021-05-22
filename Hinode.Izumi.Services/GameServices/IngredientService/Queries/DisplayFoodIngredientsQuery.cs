using MediatR;

namespace Hinode.Izumi.Services.GameServices.IngredientService.Queries
{
    public record DisplayFoodIngredientsQuery(long FoodId, long Amount = 1) : IRequest<string>;
}
