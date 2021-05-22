using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.DiscordServices.CommunityDescService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.CommunityDescService.Handlers
{
    public class DeleteContentMessageHandler : IRequestHandler<DeleteContentMessageCommand>
    {
        private readonly IConnectionManager _con;

        public DeleteContentMessageHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(DeleteContentMessageCommand request, CancellationToken cancellationToken)
        {
            var (channelId, messageId) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    delete from content_messages
                    where channel_id = @channelId
                      and message_id = @messageId",
                    new {channelId, messageId});

            return new Unit();
        }
    }
}
