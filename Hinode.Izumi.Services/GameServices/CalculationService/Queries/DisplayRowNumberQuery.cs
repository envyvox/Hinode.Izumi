using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Queries
{
    public record DisplayRowNumberQuery(long RowNumber) : IRequest<string>;
}
