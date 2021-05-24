using System.Threading;
using System.Threading.Tasks;
using Discord.Rest;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Commands
{
    public record MoveDiscordUserInChannelCommand(long UserId, RestVoiceChannel Channel) : IRequest;

    public class MoveDiscordUserInChannelHandler : IRequestHandler<MoveDiscordUserInChannelCommand>
    {
        private readonly IMediator _mediator;

        public MoveDiscordUserInChannelHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(MoveDiscordUserInChannelCommand request, CancellationToken cancellationToken)
        {
            var (userId, restVoiceChannel) = request;
            var socketGuildUser = await _mediator.Send(new GetDiscordSocketGuildUserQuery(userId), cancellationToken);

            await socketGuildUser.ModifyAsync(x => { x.Channel = restVoiceChannel; });

            return new Unit();
        }
    }
}
