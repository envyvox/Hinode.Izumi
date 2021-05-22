using MediatR;

namespace Hinode.Izumi.Services.GameServices.ReferralService.Queries
{
    public record CheckUserHasReferrerQuery(long UserId) : IRequest<bool>;
}
