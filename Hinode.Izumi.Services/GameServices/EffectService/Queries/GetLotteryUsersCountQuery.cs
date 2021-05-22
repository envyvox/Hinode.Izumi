using MediatR;

namespace Hinode.Izumi.Services.GameServices.EffectService.Queries
{
    public record GetLotteryUsersCountQuery : IRequest<long>;
}
