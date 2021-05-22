using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.UserService.Queries;
using Hinode.Izumi.Services.GameServices.UserService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.UserService.Handlers
{
    public class GetUserTitlesHandler : IRequestHandler<GetUserTitlesQuery, Dictionary<Title, UserTitleRecord>>
    {
        private readonly IConnectionManager _con;

        public GetUserTitlesHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Dictionary<Title, UserTitleRecord>> Handle(GetUserTitlesQuery request,
            CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<UserTitleRecord>(@"
                        select * from user_titles
                        where user_id = @userId",
                        new {userId = request.Id}))
                .ToDictionary(x => x.Title);
        }
    }
}
