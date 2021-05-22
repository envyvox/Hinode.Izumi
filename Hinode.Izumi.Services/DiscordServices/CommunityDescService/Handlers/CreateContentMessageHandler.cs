using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.DiscordServices.CommunityDescService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.CommunityDescService.Handlers
{
    public class CreateContentMessageHandler : IRequestHandler<CreateContentMessageCommand>
    {
        private readonly IConnectionManager _con;

        public CreateContentMessageHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(CreateContentMessageCommand request, CancellationToken cancellationToken)
        {
            var (userId, channelId, messageId) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into content_messages(user_id, channel_id, message_id)
                    values (@userId, @channelId, @messageId)",
                    new {userId, channelId, messageId});

            return new Unit();
        }
    }
}
