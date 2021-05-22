using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Queries
{
    public record DisplayProgressBarCommand(int Number) : IRequest<string>;
}
