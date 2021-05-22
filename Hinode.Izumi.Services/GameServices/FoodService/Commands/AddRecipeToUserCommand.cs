using MediatR;

namespace Hinode.Izumi.Services.GameServices.FoodService.Commands
{
    public record AddRecipeToUserCommand(long UserId, long FoodId) : IRequest;
}
