using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.DiscordServices.CommunityDescService.Records;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.CommunityDescService.Queries
{
    public record GetContentMessageByIdQuery(long Id) : IRequest<ContentMessageRecord>;

    public class GetContentMessageByIdHandler : IRequestHandler<GetContentMessageByIdQuery, ContentMessageRecord>
    {
        private readonly IConnectionManager _con;

        public GetContentMessageByIdHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<ContentMessageRecord> Handle(GetContentMessageByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<ContentMessageRecord>(@"
                    select * from content_messages
                    where id = @id",
                    new {id = request.Id});
        }
    }
}
