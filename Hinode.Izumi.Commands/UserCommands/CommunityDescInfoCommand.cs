using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.DiscordServices.CommunityDescService.Queries;
using Hinode.Izumi.Services.DiscordServices.CommunityDescService.Records;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using Hinode.Izumi.Services.DiscordServices.DiscordRoleService.Queries;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.EmoteService.Records;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.WebServices.CommandWebService.Attributes;
using Humanizer;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands
{
    [CommandCategory(CommandCategory.UserInfo)]
    [Group("доска")]
    public class CommunityDescInfoCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;
        private Dictionary<string, EmoteRecord> _emotes;

        public CommunityDescInfoCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        [Command("сообщества")]
        public async Task CommunityDescInfoTask()
        {
            _emotes = await _mediator.Send(new GetEmotesQuery());
            var channels = await _mediator.Send(new GetDiscordChannelsQuery());
            var roles = await _mediator.Send(new GetDiscordRolesQuery());
            var userMessages = await _mediator.Send(new GetContentMessagesByAuthorIdQuery(
                (long) Context.User.Id));
            var userVotes = await _mediator.Send(new GetContentAuthorVotesQuery((long) Context.User.Id));
            var userRole = await _mediator.Send(new GetDiscordUserRoleQuery(
                (long) Context.User.Id, roles[DiscordRole.ContentProvider].Id));

            var screenshotMessages = userMessages
                .Where(x => x.ChannelId == channels[DiscordChannel.Screenshots].Id)
                .ToArray();
            var memesMessages = userMessages
                .Where(x => x.ChannelId == channels[DiscordChannel.Memes].Id)
                .ToArray();
            var artMessages = userMessages
                .Where(x => x.ChannelId == channels[DiscordChannel.Arts].Id)
                .ToArray();
            var eroticMessages = userMessages
                .Where(x => x.ChannelId == channels[DiscordChannel.Erotic].Id)
                .ToArray();
            var nsfwMessages = userMessages
                .Where(x => x.ChannelId == channels[DiscordChannel.Nsfw].Id)
                .ToArray();

            var screenshotMessagesLikes = ChannelMessagesVotes(userVotes, screenshotMessages, Vote.Like);
            var screenshotMessagesDislikes = ChannelMessagesVotes(userVotes, screenshotMessages, Vote.Dislike);
            var memesMessagesLikes = ChannelMessagesVotes(userVotes, memesMessages, Vote.Like);
            var memesMessagesDislikes = ChannelMessagesVotes(userVotes, memesMessages, Vote.Dislike);
            var artMessagesLikes = ChannelMessagesVotes(userVotes, artMessages, Vote.Like);
            var artMessagesDislikes = ChannelMessagesVotes(userVotes, artMessages, Vote.Dislike);
            var eroticMessagesLikes = ChannelMessagesVotes(userVotes, eroticMessages, Vote.Like);
            var eroticMessagesDislikes = ChannelMessagesVotes(userVotes, eroticMessages, Vote.Dislike);
            var nsfwMessagesLikes = ChannelMessagesVotes(userVotes, nsfwMessages, Vote.Like);
            var nsfwMessagesDislikes = ChannelMessagesVotes(userVotes, nsfwMessages, Vote.Dislike);

            var userTotalLikes = userVotes.Count(x => x.Vote == Vote.Like);

            var embed = new EmbedBuilder()
                .WithDescription(IzumiReplyMessage.CommunityDescInfoDesc.Parse(
                    DisplayChannelInfo(screenshotMessages.Length, screenshotMessagesLikes, screenshotMessagesDislikes),
                    DisplayChannelInfo(memesMessages.Length, memesMessagesLikes, memesMessagesDislikes),
                    DisplayChannelInfo(artMessages.Length, artMessagesLikes, artMessagesDislikes),
                    DisplayChannelInfo(eroticMessages.Length, eroticMessagesLikes, eroticMessagesDislikes),
                    DisplayChannelInfo(nsfwMessages.Length, nsfwMessagesLikes, nsfwMessagesDislikes),
                    _emotes.GetEmoteOrBlank(Vote.Like.ToString()), userTotalLikes,
                    _local.Localize(Vote.Like.ToString(), userTotalLikes)));

            if (userRole is not null)
                embed.AddField(IzumiReplyMessage.CommunityDescInfoRoleFieldName.Parse(),
                    IzumiReplyMessage.CommunityDescInfoRoleFieldDesc.Parse(
                        (DateTimeOffset.Now - userRole.Expiration).TotalDays.Days()
                        .Humanize(2, new CultureInfo("ru-RU"))));

            await _mediator.Send(new SendEmbedToUserCommand(Context.User, embed));
        }

        private static long ChannelMessagesVotes(IEnumerable<ContentVoteRecord> votes,
            IEnumerable<ContentMessageRecord> messages, Vote vote)
        {
            return votes
                .Where(cv => messages
                    .Any(cm => cv.MessageId == cm.Id))
                .Count(cv => cv.Vote == vote);
        }

        private string DisplayChannelInfo(long messages, long likes, long dislikes)
        {
            return
                $"{_emotes.GetEmoteOrBlank("List")} {messages} {_local.Localize("Publication", messages)}, " +
                $"{_emotes.GetEmoteOrBlank(Vote.Like.ToString())} {likes} {_local.Localize(Vote.Like.ToString(), likes)} " +
                $"и {_emotes.GetEmoteOrBlank(Vote.Dislike.ToString())} {dislikes} {_local.Localize(Vote.Dislike.ToString(), dislikes)}.\n";
        }
    }
}
