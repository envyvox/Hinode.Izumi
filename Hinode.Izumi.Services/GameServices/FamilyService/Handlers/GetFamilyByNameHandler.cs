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
    public class GetFamilyByNameHandler : IRequestHandler<GetFamilyByNameQuery, FamilyRecord>
    {
        private readonly IConnectionManager _con;

        public GetFamilyByNameHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<FamilyRecord> Handle(GetFamilyByNameQuery request, CancellationToken cancellationToken)
        {
            var family = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<FamilyRecord>(@"
                    select * from families
                    where name = @name",
                    new {name = request.Name});

            if (family is null)
                await Task.FromException(new Exception(IzumiNullableMessage.FamilyByName.Parse()));

            return family;
        }
    }
}
