using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Queries
{
    public record GetDrinkAmountAfterMasteryProcsQuery(
            long DrinkId,
            long UserCraftingMastery,
            long Amount)
        : IRequest<long>;
}
