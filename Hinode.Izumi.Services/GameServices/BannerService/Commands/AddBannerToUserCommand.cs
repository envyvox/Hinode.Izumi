using MediatR;

namespace Hinode.Izumi.Services.GameServices.BannerService.Commands
{
    public record AddBannerToUserCommand(long UserId, long BannerId) : IRequest;
}
