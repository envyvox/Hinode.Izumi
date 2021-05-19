using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Discord;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.DiscordServices.CommunityDescService.Models;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Models;
using Hinode.Izumi.Services.DiscordServices.DiscordRoleService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;

namespace Hinode.Izumi.Services.DiscordServices.CommunityDescService.Impl
{
    [InjectableService]
    public class CommunityDescService : ICommunityDescService
    {
        private readonly IConnectionManager _con;
        private readonly IDiscordGuildService _discordGuildService;
        private readonly IEmoteService _emoteService;
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IDiscordRoleService _discordRoleService;

        public CommunityDescService(IConnectionManager con, IDiscordGuildService discordGuildService,
            IEmoteService emoteService, IDiscordEmbedService discordEmbedService,
            IDiscordRoleService discordRoleService)
        {
            _con = con;
            _discordGuildService = discordGuildService;
            _emoteService = emoteService;
            _discordEmbedService = discordEmbedService;
            _discordRoleService = discordRoleService;
        }

        public List<ulong> CommunityDescChannels(Dictionary<DiscordChannel, DiscordChannelModel> channels) =>
            new()
            {
                (ulong) channels[DiscordChannel.Screenshots].Id,
                (ulong) channels[DiscordChannel.Memes].Id,
                (ulong) channels[DiscordChannel.Arts].Id,
                (ulong) channels[DiscordChannel.Erotic].Id,
                (ulong) channels[DiscordChannel.Nsfw].Id
            };

        public async Task<ContentMessageModel> GetContentMessage(long channelId, long messageId) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<ContentMessageModel>(@"
                    select * from content_messages
                    where channel_id = @channelId
                      and message_id = @messageId",
                    new {channelId, messageId});

        public async Task<ContentMessageModel> GetContentMessage(long id) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<ContentMessageModel>(@"
                    select * from content_messages
                    where id = @id",
                    new {id});

        public async Task<Dictionary<Vote, ContentVoteModel>> GetUserVotesOnMessage(long userId, long messageId) =>
            (await _con.GetConnection()
                .QueryAsync<ContentVoteModel>(@"
                    select * from content_votes
                    where user_id = @userId
                      and message_id = @messageId",
                    new {userId, messageId}))
            .ToDictionary(x => x.Vote);

        public async Task AddContentMessage(long userId, long channelId, long messageId) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into content_messages(user_id, channel_id, message_id)
                    values (@userId, @channelId, @messageId)",
                    new {userId, channelId, messageId});

        public async Task RemoveContentMessage(long channelId, long messageId) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    delete from content_messages
                    where channel_id = @channelId
                      and message_id = @messageId",
                    new {channelId, messageId});

        public async Task AddUserVote(long userId, long messageId, Vote vote)
        {
            // добавляем реакцию пользователя к этому сообщению
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into content_votes(user_id, message_id, vote, active)
                    values (@userId, @messageId, @vote, true)",
                    new {userId, messageId, vote});
            // если пользователь поставил дизлайк - нужно проверить количество дизлайков на сообщении
            if (vote == Vote.Dislike) await CheckContentMessageDislikes(messageId);
            // если пользователь поставил лайк - нужно проверить общее количество лайков у автора
            if (vote == Vote.Like) await CheckAuthorLikes(messageId);
        }

        public async Task ActivateUserVote(long userId, long messageId, Vote vote) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    update content_votes
                    set active = true,
                        updated_at = now()
                    where user_id = @userId
                      and message_id = @messageId
                      and vote = @vote",
                    new {userId, messageId, vote});

        public async Task DeactivateUserVote(long userId, long messageId, Vote vote) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    update content_votes
                    set active = false,
                        updated_at = now()
                    where user_id = @userId
                      and message_id = @messageId
                      and vote = @vote",
                    new {userId, messageId, vote});

        private async Task CheckContentMessageDislikes(long messageId)
        {
            // получаем дизлайки сообщения
            var messageDislikes = await _con.GetConnection()
                .QueryAsync<ContentVoteModel>(@"
                        select * from content_votes
                        where message_id = @messageId
                          and vote = @voteDislike
                          and active = true",
                    new {messageId, voteDislike = Vote.Dislike});

            // если дизлайков 5 или больше
            if (messageDislikes.Count() >= 5)
            {
                // получаем это сообщение
                var contentMessage = await GetContentMessage(messageId);
                var message = await _discordGuildService.GetIUserMessage(
                    contentMessage.ChannelId, contentMessage.MessageId);

                // получаем иконки из базы
                var emotes = await _emoteService.GetEmotes();
                var embed = new EmbedBuilder()
                    .WithAuthor(IzumiReplyMessage.CommunityDescAuthor.Parse())
                    // оповещаем о том, что сообщение удалено
                    .WithDescription(IzumiReplyMessage.CommunityDescMessageDeleted.Parse(
                        emotes.GetEmoteOrBlank("Dislike"), message.Channel.Id))
                    // прикрепляем удаленное вложение
                    .WithImageUrl(message.Attachments.First().Url);

                await _discordEmbedService.SendEmbed(
                    await _discordGuildService.GetSocketUser((long) message.Author.Id), embed);

                // удаляем сообщение
                await message.DeleteAsync();
            }
        }

        private async Task CheckAuthorLikes(long messageId)
        {
            // получаем id автора сообщения
            var authorId = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<long>(@"
                    select user_id from content_messages
                    where id = @messageId",
                    new {messageId});
            // получаем количество собранных им лайков
            var authorLikes = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<long>(@"
                    select count(*) from content_votes
                    where message_id in (
                        select id from content_messages cm where cm.user_id = @authorId
                        )
                      and vote = @voteLike
                      and active = true",
                    new {authorId, voteLike = Vote.Like});

            // если автор собрал количество кратное 500
            if (authorLikes % 500 == 0)
            {
                // получаем иконки из базы
                var emotes = await _emoteService.GetEmotes();
                // получаем роли сервера
                var roles = await _discordGuildService.GetRoles();

                // добавляем ему роль на сервере
                await _discordGuildService.ToggleRoleInUser(authorId, DiscordRole.ContentProvider, true);
                // добавляем полученную роль в базу
                await _discordRoleService.AddRoleToUser(
                    authorId, roles[DiscordRole.ContentProvider].Id, DateTimeOffset.Now.AddDays(30));

                var embed = new EmbedBuilder()
                    .WithAuthor(IzumiReplyMessage.CommunityDescAuthor.Parse())
                    // уведомляем о получении роли
                    .WithDescription(IzumiReplyMessage.CommunityDescAuthorGotRole.Parse(
                        emotes.GetEmoteOrBlank("Like"), DiscordRole.ContentProvider.Name()));

                await _discordEmbedService.SendEmbed(
                    await _discordGuildService.GetSocketUser(authorId), embed);
            }
        }
    }
}
