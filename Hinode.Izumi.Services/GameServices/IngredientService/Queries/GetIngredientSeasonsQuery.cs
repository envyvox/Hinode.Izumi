using System.Collections.Generic;
using Hinode.Izumi.Data.Enums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.IngredientService.Queries
{
    public record GetIngredientSeasonsQuery(IngredientCategory Category, long IngredientId) : IRequest<List<Season>>;
}
