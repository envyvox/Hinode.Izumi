using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.FieldService.Queries;
using Hinode.Izumi.Services.GameServices.FieldService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FieldService.Handlers
{
    public class GetUserFieldsHandler : IRequestHandler<GetUserFieldsQuery, UserFieldRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetUserFieldsHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<UserFieldRecord[]> Handle(GetUserFieldsQuery request, CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<UserFieldRecord>(@"
                        select * from user_fields
                        where user_id = @userId
                        order by field_id",
                        new {userId = request.UserId}))
                .ToArray();
        }
    }
}
