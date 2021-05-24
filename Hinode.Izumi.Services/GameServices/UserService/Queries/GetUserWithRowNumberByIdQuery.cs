using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.UserService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.UserService.Queries
{
    public record GetUserWithRowNumberByIdQuery(long Id) : IRequest<UserWithRowNumberRecord>;

    public class GetUserWithRowNumberByIdHandler
        : IRequestHandler<GetUserWithRowNumberByIdQuery, UserWithRowNumberRecord>
    {
        private readonly IConnectionManager _con;

        public GetUserWithRowNumberByIdHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<UserWithRowNumberRecord> Handle(GetUserWithRowNumberByIdQuery request,
            CancellationToken cancellationToken)
        {
            var user = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserWithRowNumberRecord>(@"
                    select * from (
                        select *,
                               row_number() over (order by points desc, created_at desc) as RowNumber
                        from users) tmp
                    where tmp.id = @userId",
                    new {userId = request.Id});


            if (user is null)
                await Task.FromException(new Exception(IzumiNullableMessage.UserWithId.Parse()));

            return user;
        }
    }
}
