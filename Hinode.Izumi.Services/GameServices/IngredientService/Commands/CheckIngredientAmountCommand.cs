using Hinode.Izumi.Data.Enums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.IngredientService.Commands
{
    public record CheckIngredientAmountCommand(
            long UserId,
            IngredientCategory Category,
            long IngredientId,
            long IngredientAmount,
            long CraftingAmount = 1)
        : IRequest;
}
