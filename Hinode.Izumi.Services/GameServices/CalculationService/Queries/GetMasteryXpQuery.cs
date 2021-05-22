using Hinode.Izumi.Data.Enums.PropertyEnums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Queries
{
    public record GetMasteryXpQuery(
            MasteryXpProperty Property,
            long UserMasteryAmount,
            long ItemsCount = 1)
        : IRequest<double>;
}
