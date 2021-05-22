using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.FamilyService.Queries;
using Hinode.Izumi.Services.GameServices.FamilyService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Handlers
{
    public class GetUserFamilyHandler : IRequestHandler<GetUserFamilyQuery, UserFamilyRecord>
    {
        private readonly IConnectionManager _con;

        public GetUserFamilyHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<UserFamilyRecord> Handle(GetUserFamilyQuery request, CancellationToken cancellationToken)
        {
            var userFamily = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserFamilyRecord>(@"
                    select * from user_families
                    where user_id = @userId",
                    new {userId = request.UserId});

            if (userFamily is null)
                await Task.FromException(new Exception(IzumiNullableMessage.UserFamily.Parse()));

            return userFamily;
        }
    }
}
