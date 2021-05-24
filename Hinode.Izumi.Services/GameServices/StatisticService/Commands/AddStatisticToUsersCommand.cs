using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.StatisticService.Commands
{
    public record AddStatisticToUsersCommand(long[] UsersId, Statistic Statistic, long Amount = 1) : IRequest;

    public class AddStatisticToUsersHandler : IRequestHandler<AddStatisticToUsersCommand>
    {
        private readonly IConnectionManager _con;

        public AddStatisticToUsersHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(AddStatisticToUsersCommand request, CancellationToken cancellationToken)
        {
            var (usersId, statistic, amount) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_statistics as us (user_id, statistic, amount)
                    values (unnest(array[@usersId]), @statistic, @amount)
                    on conflict (user_id, statistic) do update
                        set amount = us.amount + @amount,
                            updated_at = now()",
                    new {usersId, statistic, amount});

            return new Unit();
        }
    }
}
