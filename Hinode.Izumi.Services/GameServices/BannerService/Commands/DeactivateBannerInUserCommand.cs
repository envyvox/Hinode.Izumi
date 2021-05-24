using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.BannerService.Commands
{
    public record DeactivateBannerInUserCommand(long UserId, long BannerId) : IRequest;

    public class DeactivateBannerInUserHandler : IRequestHandler<DeactivateBannerInUserCommand>
    {
        private readonly IConnectionManager _con;

        public DeactivateBannerInUserHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(DeactivateBannerInUserCommand request, CancellationToken cancellationToken)
        {
            var (userId, bannerId) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    update user_banners
                    set active = false,
                        updated_at = now()
                    where user_id = @userId
                      and banner_id = @bannerId",
                    new {userId, bannerId});

            return new Unit();
        }
    }
}
