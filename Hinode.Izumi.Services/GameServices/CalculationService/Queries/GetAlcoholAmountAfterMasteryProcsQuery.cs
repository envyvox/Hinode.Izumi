using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Queries
{
    public record GetAlcoholAmountAfterMasteryProcsQuery(
            long AlcoholId,
            long UserCraftingMastery,
            long Amount)
        : IRequest<long>;
}
