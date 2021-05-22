using MediatR;

namespace Hinode.Izumi.Services.GameServices.BannerService.Commands
{
    public record ActivateBannerInUserCommand(long UserId, long BannerId) : IRequest;
}
