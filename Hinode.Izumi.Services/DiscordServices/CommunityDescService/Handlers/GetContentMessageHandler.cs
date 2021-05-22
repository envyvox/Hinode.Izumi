using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.DiscordServices.CommunityDescService.Queries;
using Hinode.Izumi.Services.DiscordServices.CommunityDescService.Records;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.CommunityDescService.Handlers
{
    public class GetContentMessageHandler : IRequestHandler<GetContentMessageQuery, ContentMessageRecord>
    {
        private readonly IConnectionManager _con;

        public GetContentMessageHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<ContentMessageRecord> Handle(GetContentMessageQuery request,
            CancellationToken cancellationToken)
        {
            var (channelId, messageId) = request;
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<ContentMessageRecord>(@"
                    select * from content_messages
                    where channel_id = @channelId
                      and message_id = @messageId",
                    new {channelId, messageId});
        }
    }
}
