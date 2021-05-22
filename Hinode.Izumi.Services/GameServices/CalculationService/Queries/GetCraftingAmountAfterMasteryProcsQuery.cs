using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Queries
{
    public record GetCraftingAmountAfterMasteryProcsQuery(
            long CraftingId,
            long UserCraftingMastery,
            long Amount)
        : IRequest<long>;
}
