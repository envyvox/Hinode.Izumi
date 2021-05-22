using MediatR;

namespace Hinode.Izumi.Services.GameServices.BannerService.Queries
{
    public record CheckUserHasBannerQuery(long UserId, long BannerId) : IRequest<bool>;
}
