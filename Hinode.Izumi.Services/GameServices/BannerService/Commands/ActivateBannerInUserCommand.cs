using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.BannerService.Commands
{
    public record ActivateBannerInUserCommand(long UserId, long BannerId) : IRequest;

    public class ActivateBannerInUserHandler : IRequestHandler<ActivateBannerInUserCommand>
    {
        private readonly IConnectionManager _con;

        public ActivateBannerInUserHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(ActivateBannerInUserCommand request, CancellationToken cancellationToken)
        {
            var (userId, bannerId) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    update user_banners
                    set active = true,
                        updated_at = now()
                    where user_id = @userId
                      and banner_id = @bannerId",
                    new {userId, bannerId});

            return new Unit();
        }
    }
}
