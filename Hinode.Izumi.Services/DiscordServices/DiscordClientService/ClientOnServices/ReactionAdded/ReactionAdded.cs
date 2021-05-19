using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.CommunityDescService;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;

namespace Hinode.Izumi.Services.DiscordServices.DiscordClientService.ClientOnServices.ReactionAdded
{
    [InjectableService]
    public class ReactionAdded : IReactionAdded
    {
        private readonly IDiscordGuildService _discordGuildService;
        private readonly ICommunityDescService _communityDescService;
        private readonly IEmoteService _emoteService;

        public ReactionAdded(IDiscordGuildService discordGuildService, IEmoteService emoteService,
            ICommunityDescService communityDescService)
        {
            _discordGuildService = discordGuildService;
            _communityDescService = communityDescService;
            _emoteService = emoteService;
        }

        public async Task Execute(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel socketMessageChannel,
            SocketReaction socketReaction)
        {
            // убеждаемся что мы получили сообщение
            var msg = await message.GetOrDownloadAsync();
            // игнорируем реакции поставленные ботом
            if (socketReaction.User.Value.IsBot) return;

            // получаем каналы сервера
            var channels = await _discordGuildService.GetChannels();
            // получаем каналы доски сообщества
            var communityDescChannels = _communityDescService.CommunityDescChannels(channels);

            // если поставленная реакция находится в получении ролей
            if (socketMessageChannel.Id == (ulong) channels[DiscordChannel.GetRoles].Id ||
                // или в регистрации
                socketMessageChannel.Id == (ulong) channels[DiscordChannel.Registration].Id)
            {
                // определяем какую роль необходимо выдать в зависимости от названия реакции
                var role = socketReaction.Emote.Name switch
                {
                    // роли оповещений событий
                    "NumOne" => DiscordRole.AllEvents,
                    "NumTwo" => DiscordRole.DailyEvents,
                    "NumThree" => DiscordRole.WeeklyEvents,
                    "NumFour" => DiscordRole.MonthlyEvents,
                    "NumFive" => DiscordRole.YearlyEvents,
                    "NumSix" => DiscordRole.UniqueEvents,
                    // игровые роли
                    "GenshinImpact" => DiscordRole.GenshinImpact,
                    "LeagueOfLegends" => DiscordRole.LeagueOfLegends,
                    "TeamfightTactics" => DiscordRole.TeamfightTactics,
                    "Valorant" => DiscordRole.Valorant,
                    "ApexLegends" => DiscordRole.ApexLegends,
                    "LostArk" => DiscordRole.LostArk,
                    "Dota" => DiscordRole.Dota,
                    "Osu" => DiscordRole.Osu,
                    "AmongUs" => DiscordRole.AmongUs,
                    _ => throw new ArgumentOutOfRangeException()
                };

                // проверяем есть ли у пользователя уже эта роль
                var hasRole = await _discordGuildService.CheckRoleInUser((long) socketReaction.UserId, role);
                // если есть - снимаем, если нет - добавляем
                await _discordGuildService.ToggleRoleInUser((long) socketReaction.UserId, role, !hasRole);

                // снимаем поставленную пользователем реакцию
                await msg.RemoveReactionAsync(socketReaction.Emote, socketReaction.User.Value);
            }

            // если сообщение реакции находится в канале доски сообщества
            if (communityDescChannels.Contains(socketMessageChannel.Id))
            {
                // если название реакции не соответствует названиям голосования - игнорируем
                if (socketReaction.Emote.Name != "Like" &&
                    socketReaction.Emote.Name != "Dislike") return;

                // получаем сообщение из базы
                var contentMessage = await _communityDescService.GetContentMessage(
                    (long) socketMessageChannel.Id, (long) socketReaction.MessageId);

                // если пользователь поставил реакцию на свое же сообщение
                if (socketReaction.UserId == (ulong) contentMessage.UserId)
                {
                    // снимаем реакцию с сообщения
                    await msg.RemoveReactionAsync(socketReaction.Emote, socketReaction.UserId);
                }
                else
                {
                    // определяем какую реакцию поставил пользователь
                    var vote = socketReaction.Emote.Name == "Like" ? Vote.Like : Vote.Dislike;
                    // определяем ее противоположную реакцию
                    var antiVote = vote == Vote.Like ? Vote.Dislike : Vote.Like;
                    // получаем реакции пользователя на это сообщение
                    var userVotes = await _communityDescService.GetUserVotesOnMessage(
                        (long) socketReaction.UserId, contentMessage.Id);
                    // получаем иконки из базы
                    var emotes = await _emoteService.GetEmotes();

                    // если пользователь ставил до этого противоположную реакцию ее нужно снять
                    if (userVotes.ContainsKey(antiVote) && userVotes[antiVote].Active)
                    {
                        // это действие вызовет ReactionRemoved Handler и деактивирует реакцию в базе
                        await msg.RemoveReactionAsync(
                            antiVote == Vote.Like
                                ? Emote.Parse(emotes.GetEmoteOrBlank("Like"))
                                : Emote.Parse(emotes.GetEmoteOrBlank("Dislike")),
                            socketReaction.UserId);
                    }

                    // если пользователь уже ставил эту реацию на это сообщение
                    if (userVotes.ContainsKey(vote))
                    {
                        // активируем реакцию пользотеля в базе
                        await _communityDescService.ActivateUserVote(
                            (long) socketReaction.UserId, contentMessage.Id, vote);
                    }
                    // если он ставит эту реацию впервые
                    else
                    {
                        // добавляем реакцию пользователя в базу
                        await _communityDescService.AddUserVote(
                            (long) socketReaction.UserId, contentMessage.Id, vote);
                    }
                }
            }
        }
    }
}
