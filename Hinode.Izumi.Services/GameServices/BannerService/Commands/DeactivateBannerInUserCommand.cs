using MediatR;

namespace Hinode.Izumi.Services.GameServices.BannerService.Commands
{
    public record DeactivateBannerInUserCommand(long UserId, long BannerId) : IRequest;
}
