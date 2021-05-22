using MediatR;

namespace Hinode.Izumi.Services.GameServices.ReferralService.Queries
{
    public record GetUserReferralCountQuery(long UserId) : IRequest<long>;
}
