using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.FieldService.Queries;
using Hinode.Izumi.Services.GameServices.FieldService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FieldService.Handlers
{
    public class GetUserFieldHandler : IRequestHandler<GetUserFieldQuery, UserFieldRecord>
    {
        private readonly IConnectionManager _con;

        public GetUserFieldHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<UserFieldRecord> Handle(GetUserFieldQuery request, CancellationToken cancellationToken)
        {
            var (userId, fieldId) = request;
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserFieldRecord>(@"
                    select * from user_fields
                    where user_id = @userId
                      and field_id = @fieldId",
                    new {userId, fieldId});
        }
    }
}
