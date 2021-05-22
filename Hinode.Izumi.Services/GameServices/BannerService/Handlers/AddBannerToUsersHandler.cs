using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.BannerService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.BannerService.Handlers
{
    public class AddBannerToUsersHandler : IRequestHandler<AddBannerToUsersCommand>
    {
        private readonly IConnectionManager _con;

        public AddBannerToUsersHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(AddBannerToUsersCommand request, CancellationToken cancellationToken)
        {
            var (usersId, bannerId) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_banners(user_id, banner_id, active)
                    values (unnest(array[@usersId]), @bannerId, false)
                    on conflict (user_id, banner_id) do nothing",
                    new {usersId, bannerId});

            return new Unit();
        }
    }
}
