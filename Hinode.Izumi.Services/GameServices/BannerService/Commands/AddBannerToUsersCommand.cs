using MediatR;

namespace Hinode.Izumi.Services.GameServices.BannerService.Commands
{
    public record AddBannerToUsersCommand(long[] UsersId, long BannerId) : IRequest;
}
