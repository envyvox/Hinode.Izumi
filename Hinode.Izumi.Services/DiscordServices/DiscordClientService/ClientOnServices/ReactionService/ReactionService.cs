using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;

namespace Hinode.Izumi.Services.DiscordServices.DiscordClientService.ClientOnServices.ReactionService
{
    [InjectableService]
    public class ReactionService : IReactionService
    {
        private readonly IDiscordGuildService _discordGuildService;

        public ReactionService(IDiscordGuildService discordGuildService)
        {
            _discordGuildService = discordGuildService;
        }

        public async Task Execute(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel,
            SocketReaction reaction)
        {
            // получаем каналы сервера
            var channels = await _discordGuildService.GetChannels();

            // игнорируем реакции от ботов и реакции в непредусмотренных каналах
            if (reaction.User.Value.IsBot ||
                channel.Id != (ulong) channels[DiscordChannel.GetRoles].Id &&
                channel.Id != (ulong) channels[DiscordChannel.Registration].Id &&
                channel.Id != (ulong) channels[DiscordChannel.Test].Id)
                return;

            // определяем какую роль необходимо выдать в зависимости от названия реакции
            var role = reaction.Emote.Name switch
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
            var hasRole = await _discordGuildService.CheckRoleInUser((long) reaction.UserId, role);

            // если есть - снимаем
            if (hasRole) await _discordGuildService.ToggleRoleInUser((long) reaction.UserId, role, false);
            // если нет - добавляем
            else await _discordGuildService.ToggleRoleInUser((long) reaction.UserId, role, true);

            // получаем сообщение на котором была поставлена реакция
            var msg = await _discordGuildService.GetIUserMessage((long) reaction.Channel.Id, (long) message.Id);
            // снимаем поставленную пользователем реакцию
            await msg.RemoveReactionAsync(reaction.Emote, reaction.User.Value);
        }
    }
}
