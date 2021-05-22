using Hinode.Izumi.Services.GameServices.BannerService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.BannerService.Queries
{
    public record GetUserActiveBannerQuery(long UserId) : IRequest<BannerRecord>;
}
