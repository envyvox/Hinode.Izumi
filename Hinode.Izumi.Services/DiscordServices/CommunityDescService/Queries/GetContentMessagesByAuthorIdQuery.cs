using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.DiscordServices.CommunityDescService.Records;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.CommunityDescService.Queries
{
    public record GetContentMessagesByAuthorIdQuery(long UserId) : IRequest<ContentMessageRecord[]>;

    public class GetContentMessagesByAuthorIdHandler
        : IRequestHandler<GetContentMessagesByAuthorIdQuery, ContentMessageRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetContentMessagesByAuthorIdHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<ContentMessageRecord[]> Handle(GetContentMessagesByAuthorIdQuery request,
            CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<ContentMessageRecord>(@"
                        select * from content_messages
                        where user_id = @userId",
                        new {userId = request.UserId}))
                .ToArray();
        }
    }
}
