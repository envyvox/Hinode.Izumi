using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.CommunityDescService.Commands
{
    public record DeactivateUserVoteCommand(long UserId, long ContentMessageId, Vote Vote) : IRequest;

    public class DeactivateUserVoteHandler : IRequestHandler<DeactivateUserVoteCommand>
    {
        private readonly IConnectionManager _con;

        public DeactivateUserVoteHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(DeactivateUserVoteCommand request, CancellationToken cancellationToken)
        {
            var (userId, messageId, vote) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    update content_votes
                    set active = false,
                        updated_at = now()
                    where user_id = @userId
                      and message_id = @messageId
                      and vote = @vote",
                    new {userId, messageId, vote});

            return new Unit();
        }
    }
}
