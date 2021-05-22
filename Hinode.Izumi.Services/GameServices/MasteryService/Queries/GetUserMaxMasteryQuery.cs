using MediatR;

namespace Hinode.Izumi.Services.GameServices.MasteryService.Queries
{
    public record GetUserMaxMasteryQuery(long UserId) : IRequest<double>;
}
