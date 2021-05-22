using MediatR;

namespace Hinode.Izumi.Services.GameServices.FoodService.Queries
{
    public record CheckUserHasRecipeQuery(long UserId, long FoodId) : IRequest<bool>;
}
