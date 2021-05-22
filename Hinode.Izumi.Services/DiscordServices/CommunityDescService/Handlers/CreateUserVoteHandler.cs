using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.DiscordServices.CommunityDescService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.CommunityDescService.Handlers
{
    public class CreateUserVoteHandler : IRequestHandler<CreateUserVoteCommand>
    {
        private readonly IConnectionManager _con;
        private readonly IMediator _mediator;

        public CreateUserVoteHandler(IConnectionManager con, IMediator mediator)
        {
            _con = con;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CreateUserVoteCommand request, CancellationToken cancellationToken)
        {
            var (userId, messageId, vote) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into content_votes(user_id, message_id, vote, active)
                    values (@userId, @messageId, @vote, true)",
                    new {userId, messageId, vote});

            if (vote == Vote.Dislike)
                await _mediator.Send(new CheckContentMessageDislikesCommand(messageId), cancellationToken);
            if (vote == Vote.Like)
                await _mediator.Send(new CheckContentMessageAuthorLikesCommand(messageId), cancellationToken);

            return new Unit();
        }
    }
}
