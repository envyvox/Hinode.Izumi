using Hinode.Izumi.Services.GameServices.UserService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.ReferralService.Queries
{
    public record GetUserReferralsQuery(long UserId) : IRequest<UserRecord[]>;
}
