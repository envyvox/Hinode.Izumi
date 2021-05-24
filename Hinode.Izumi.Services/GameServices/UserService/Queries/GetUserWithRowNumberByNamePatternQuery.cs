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
    public record GetUserWithRowNumberByNamePatternQuery(string NamePattern) : IRequest<UserWithRowNumberRecord>;

    public class GetUserWithRowNumberByNamePatternHandler
        : IRequestHandler<GetUserWithRowNumberByNamePatternQuery, UserWithRowNumberRecord>
    {
        private readonly IConnectionManager _con;

        public GetUserWithRowNumberByNamePatternHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<UserWithRowNumberRecord> Handle(GetUserWithRowNumberByNamePatternQuery request,
            CancellationToken cancellationToken)
        {
            var user = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserWithRowNumberRecord>(@"
                    select * from (
                        select *,
                               row_number() over (order by points desc, created_at desc) as RowNumber
                        from users) tmp
                    where tmp.name ilike '%'||@namePattern||'%'",
                    new {namePattern = request.NamePattern});


            if (user is null)
                await Task.FromException(new Exception(IzumiNullableMessage.UserWithName.Parse()));

            return user;
        }
    }
}
