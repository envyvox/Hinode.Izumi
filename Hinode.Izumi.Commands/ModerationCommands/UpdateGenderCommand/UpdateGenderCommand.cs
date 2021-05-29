using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.UserService.Commands;
using MediatR;

namespace Hinode.Izumi.Commands.ModerationCommands.UpdateGenderCommand
{
    [InjectableService]
    public class UpdateGenderCommand : IUpdateGenderCommand
    {
        private readonly IMediator _mediator;

        public UpdateGenderCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Execute(SocketCommandContext context, long userId, Gender gender)
        {
            await _mediator.Send(new UpdateUserGenderCommand(userId, gender));

            var emotes = await _mediator.Send(new GetEmotesQuery());
            var socketUser = await _mediator.Send(new GetDiscordSocketUserQuery(userId));

            var embed = new EmbedBuilder()
                .WithDescription(IzumiReplyMessage.ModGenderDesc.Parse(
                    socketUser.Mention, emotes.GetEmoteOrBlank(gender.Emote()), gender.Localize()));

            await _mediator.Send(new SendEmbedToChannelCommand(DiscordChannel.ModeratorChat, embed));

            var notifyEmbed = new EmbedBuilder()
                .WithDescription(IzumiReplyMessage.ModGenderNotifyDesc.Parse(
                    emotes.GetEmoteOrBlank(gender.Emote()), gender.Localize()));

            await _mediator.Send(new SendEmbedToUserCommand(socketUser, notifyEmbed));
        }
    }
}
