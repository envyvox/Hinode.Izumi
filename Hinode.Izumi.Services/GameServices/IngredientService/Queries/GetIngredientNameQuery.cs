using Hinode.Izumi.Data.Enums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.IngredientService.Queries
{
    public record GetIngredientNameQuery(IngredientCategory Category, long IngredientId) : IRequest<string>;
}
