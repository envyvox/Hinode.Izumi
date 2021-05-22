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
    public class GetFamilyInviteByIdHandler : IRequestHandler<GetFamilyInviteByIdQuery, FamilyInviteRecord>
    {
        private readonly IConnectionManager _con;

        public GetFamilyInviteByIdHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<FamilyInviteRecord> Handle(GetFamilyInviteByIdQuery request,
            CancellationToken cancellationToken)
        {
            var res = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<FamilyInviteRecord>(@"
                    select * from family_invites
                    where id = @id",
                    new {id = request.Id});

            if (res is not null) return res;

            await Task.FromException(new Exception(IzumiNullableMessage.FamilyInvite.Parse()));
            return null;
        }
    }
}
