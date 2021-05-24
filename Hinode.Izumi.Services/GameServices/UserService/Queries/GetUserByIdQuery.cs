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
    public record GetUserByIdQuery(long Id) : IRequest<UserRecord>;

    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserRecord>
    {
        private readonly IConnectionManager _con;

        public GetUserByIdHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<UserRecord> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserRecord>(@"
                    select * from users
                    where id = @userId",
                    new {userId = request.Id});

            if (user is null)
                await Task.FromException(new Exception(IzumiNullableMessage.UserWithId.Parse()));

            return user;
        }
    }
}
