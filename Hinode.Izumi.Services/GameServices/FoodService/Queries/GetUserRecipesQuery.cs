using Hinode.Izumi.Services.GameServices.FoodService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FoodService.Queries
{
    public record GetUserRecipesQuery(long UserId) : IRequest<FoodRecord[]>;
}
