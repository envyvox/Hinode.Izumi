using MediatR;

namespace Hinode.Izumi.Services.GameServices.ReferralService.Commands
{
    public record CreateUserReferrerCommand(long UserId, long ReferrerId) : IRequest;
}
