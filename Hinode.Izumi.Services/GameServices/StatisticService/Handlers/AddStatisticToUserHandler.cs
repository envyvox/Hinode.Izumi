﻿using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.StatisticService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.StatisticService.Handlers
{
    public class AddStatisticToUserHandler : IRequestHandler<AddStatisticToUserCommand>
    {
        private readonly IConnectionManager _con;

        public AddStatisticToUserHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(AddStatisticToUserCommand request, CancellationToken cancellationToken)
        {
            var (userId, statistic, amount) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_statistics as us (user_id, statistic, amount)
                    values (@userId, @statistic, @amount)
                    on conflict (user_id, statistic) do update
                        set amount = us.amount + @amount,
                            updated_at = now()",
                    new {userId, statistic, amount});

            return new Unit();
        }
    }
}
