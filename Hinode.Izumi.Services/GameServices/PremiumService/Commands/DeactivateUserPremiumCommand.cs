using System.Threading;
using System.Threading.Tasks;
using Discord;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.PremiumService.Commands
{
    public record DeactivateUserPremiumCommand(long UserId) : IRequest;

    public class DeactivateUserPremiumHandler : IRequestHandler<DeactivateUserPremiumCommand>
    {
        private readonly IMediator _mediator;

        public DeactivateUserPremiumHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(DeactivateUserPremiumCommand request, CancellationToken ct)
        {
            var emotes = await _mediator.Send(new GetEmotesQuery(), ct);

            await _mediator.Send(new UpdateUserPremiumStatusCommand(
                request.UserId, false), ct);
            await _mediator.Send(new DeleteUserPremiumPropertiesCommand(request.UserId), ct);

            var embed = new EmbedBuilder()
                .WithDescription(IzumiReplyMessage.DeactivatePremiumNotify.Parse(
                    emotes.GetEmoteOrBlank("Premium")));

            await _mediator.Send(new SendEmbedToUserCommand(
                await _mediator.Send(new GetDiscordSocketUserQuery(request.UserId), ct), embed), ct);

            return new Unit();
        }
    }
}
