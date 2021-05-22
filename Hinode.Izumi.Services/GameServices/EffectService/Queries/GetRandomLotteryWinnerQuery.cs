using Hinode.Izumi.Services.GameServices.UserService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.EffectService.Queries
{
    public record GetRandomLotteryWinnerQuery : IRequest<UserRecord>;
}
