using System;
using System.Globalization;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hangfire;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.BackgroundJobs.MuteJob;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using Humanizer;
using MediatR;

namespace Hinode.Izumi.Commands.ModerationCommands.MuteCommand
{
    [InjectableService]
    public class MuteCommand : IMuteCommand
    {
        private readonly IMediator _mediator;
        private readonly Random _random = new();

        public MuteCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Execute(SocketCommandContext context, long userId, long duration, string reason = null)
        {
            var channels = await _mediator.Send(new GetDiscordChannelsQuery());
            var user = await _mediator.Send(new GetDiscordSocketUserQuery(userId));
            var admin = await _mediator.Send(
                new CheckDiscordRoleInUserQuery((long) context.User.Id, DiscordRole.Administration));

            await _mediator.Send(new AddDiscordRoleToUserCommand(userId, DiscordRole.Mute));

            var embed = new EmbedBuilder()
                .WithTitle(IzumiReplyMessage.MuteTitle.Parse(
                    _random.Next(1000, 10000)))
                .WithDescription(IzumiReplyMessage.MuteDesc.Parse(
                    user.Mention, user.Username, channels[DiscordChannel.Rules].Id))
                .AddField(IzumiReplyMessage.TimeFieldName.Parse(),
                    duration.Minutes().Humanize(2, new CultureInfo("ru-RU")), true)
                .AddField(IzumiReplyMessage.MuteReasonFieldName.Parse(),
                    reason ?? IzumiReplyMessage.MuteReasonNull.Parse(), true)
                .AddField(IzumiReplyMessage.MuteSignatureFieldName.Parse(),
                    $"**{(admin ? DiscordRole.Administration.Name() : DiscordRole.Moderator.Name())}** {context.User.Mention} `@{context.User.Username}`");

            await _mediator.Send(new SendEmbedToChannelCommand(DiscordChannel.Chat, embed));
            await _mediator.Send(new SendEmbedToChannelCommand(DiscordChannel.LogMute, embed));

            BackgroundJob.Schedule<IMuteJob>(
                x => x.Unmute(userId),
                TimeSpan.FromMinutes(duration));
        }
    }
}
