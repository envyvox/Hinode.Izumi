using Hinode.Izumi.Services.GameServices.UserService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.ReferralService.Queries
{
    public record GetUserReferrerQuery(long UserId) : IRequest<UserRecord>;
}
