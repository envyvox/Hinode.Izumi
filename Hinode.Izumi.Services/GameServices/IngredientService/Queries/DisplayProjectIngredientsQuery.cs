using MediatR;

namespace Hinode.Izumi.Services.GameServices.IngredientService.Queries
{
    public record DisplayProjectIngredientsQuery(long ProjectId, long Amount = 1) : IRequest<string>;
}
