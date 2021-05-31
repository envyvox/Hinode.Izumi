using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using Hinode.Izumi.Services.DiscordServices.DiscordRoleService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Humanizer;
using Humanizer.Localisation;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.PremiumService.Commands
{
    public record ActivateUserPremiumCommand(long UserId, long Days) : IRequest;

    public class ActivateUserPremiumHandler : IRequestHandler<ActivateUserPremiumCommand>
    {
        private readonly IMediator _mediator;

        public ActivateUserPremiumHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(ActivateUserPremiumCommand request, CancellationToken ct)
        {
            var (userId, days) = request;
            var emotes = await _mediator.Send(new GetEmotesQuery(), ct);
            var roles = await _mediator.Send(new GetDiscordRolesQuery(), ct);

            await _mediator.Send(new AddDiscordRoleToUserCommand(
                userId, DiscordRole.Premium), ct);
            await _mediator.Send(new AddDiscordUserRoleToDbCommand(
                userId, roles[DiscordRole.Premium].Id, days), ct);
            await _mediator.Send(new UpdateUserPremiumStatusCommand(
                userId, true), ct);
            await _mediator.Send(new CreateUserPremiumPropertiesCommand(
                userId), ct);

            var embed = new EmbedBuilder()
                .WithDescription(IzumiReplyMessage.ActivatePremiumNotify.Parse(
                    emotes.GetEmoteOrBlank("Premium"),
                    days.Days().Humanize(1, new CultureInfo("ru-RU"), TimeUnit.Day)));

            await _mediator.Send(new SendEmbedToUserCommand(
                await _mediator.Send(new GetDiscordSocketUserQuery(userId), ct), embed), ct);

            return new Unit();
        }
    }
}
