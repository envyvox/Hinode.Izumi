using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.BannerService.Commands
{
    public record AddBannerToUserCommand(long UserId, long BannerId) : IRequest;

    public class AddBannerToUserHandler : IRequestHandler<AddBannerToUserCommand>
    {
        private readonly IConnectionManager _con;

        public AddBannerToUserHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(AddBannerToUserCommand request, CancellationToken cancellationToken)
        {
            var (userId, bannerId) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_banners(user_id, banner_id, active)
                    values (@userId, @bannerId, false)
                    on conflict (user_id, banner_id) do nothing",
                    new {userId, bannerId});

            return new Unit();
        }
    }
}
