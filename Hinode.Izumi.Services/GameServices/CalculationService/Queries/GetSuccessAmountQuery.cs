using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Queries
{
    public record GetSuccessAmountQuery(long Chance, long DoubleChance, long Amount) : IRequest<long>;
}
