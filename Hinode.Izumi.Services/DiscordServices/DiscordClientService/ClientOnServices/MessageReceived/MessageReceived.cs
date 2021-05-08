using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Hangfire;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.AchievementEnums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.BackgroundJobs.DiscordJob;
using Hinode.Izumi.Services.DiscordServices.CommunityDescService;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.AchievementService;
using Hinode.Izumi.Services.RpgServices.StatisticService;
using Hinode.Izumi.Services.RpgServices.UserService;

namespace Hinode.Izumi.Services.DiscordServices.DiscordClientService.ClientOnServices.MessageReceived
{
    [InjectableService]
    public class MessageReceived : IMessageReceived
    {
        private readonly IDiscordGuildService _discordGuildService;
        private readonly IStatisticService _statisticService;
        private readonly IUserService _userService;
        private readonly IAchievementService _achievementService;
        private readonly IEmoteService _emoteService;
        private readonly ICommunityDescService _communityDescService;

        public MessageReceived(IDiscordGuildService discordGuildService, IStatisticService statisticService,
            IUserService userService, IAchievementService achievementService, IEmoteService emoteService,
            ICommunityDescService communityDescService)
        {
            _discordGuildService = discordGuildService;
            _statisticService = statisticService;
            _userService = userService;
            _achievementService = achievementService;
            _emoteService = emoteService;
            _communityDescService = communityDescService;
        }

        public async Task Execute(DiscordSocketClient socketClient, SocketMessage socketMessage)
        {
            // игнорируем сообщениния от ботов
            if (socketMessage.Author.IsBot) return;

            // получаем каналы сервера
            var channels = await _discordGuildService.GetChannels();
            // получаем из них каналы доски сообщества
            var communityDescChannels = _communityDescService.CommunityDescChannels(channels);

            // если сообщение находится в канале доски сообщества
            if (communityDescChannels.Contains(socketMessage.Channel.Id))
            {
                // проверяем его на наличие вложений
                var hasAttachment = CheckAttachment(socketMessage);
                // если они есть - отправляем сообщение в сервис доски сообщества
                if (hasAttachment)
                {
                    // добавляем реакции голосования
                    await AddVotes((IUserMessage) socketMessage);
                    // добавляем сообщение в базу
                    await _communityDescService.AddContentMessage(
                        (long) socketMessage.Author.Id, (long) socketMessage.Channel.Id, (long) socketMessage.Id);
                }

                // если нет - удаляем сообщение
                else await DeleteMessage(socketMessage);
            }

            // если сообщение находится в канале предложения
            if (socketMessage.Channel.Id == (ulong) channels[DiscordChannel.Suggestions].Id)
                // добавляем реакции голосования
                await AddVotes((IUserMessage) socketMessage);

            // если это сообщение в общении
            if (socketMessage.Channel.Id == (ulong) channels[DiscordChannel.Chat].Id)
            {
                // проверяем есть ли у пользователя нитро буст
                var hasNitroRole = await _discordGuildService.CheckRoleInUser(
                    (long) socketMessage.Author.Id, DiscordRole.Nitro);
                // проверяем есть ли у пользователя роль модератора
                var hasModeratorRole = await _discordGuildService.CheckRoleInUser(
                    (long) socketMessage.Author.Id, DiscordRole.Moderator);
                // проверяем есть ли у пользователя роль ивент-менеджера
                var hasEventManagerRole = await _discordGuildService.CheckRoleInUser(
                    (long) socketMessage.Author.Id, DiscordRole.EventManager);
                // проверяем есть ли у пользователя роль администратора
                var hasAdministrationRole = await _discordGuildService.CheckRoleInUser(
                    (long) socketMessage.Author.Id, DiscordRole.Administration);
                var hasStaffRole = hasModeratorRole || hasEventManagerRole || hasAdministrationRole;
                // проверяем зарегистрирован ли пользователь в игровом мире
                var checkUser = await _userService.CheckUser((long) socketMessage.Author.Id);

                // если сообщение имеет вложение или ссылку и пользователь не имеет роли нитро-буста или стафа
                if (CheckAttachment(socketMessage) && !(hasNitroRole || hasStaffRole))
                    // удаляем сообщение через 10 минут
                    BackgroundJob.Schedule<IDiscordJob>(
                        x => x.DeleteMessage((long) socketMessage.Channel.Id, (long) socketMessage.Id),
                        TimeSpan.FromMinutes(10));

                if (checkUser)
                {
                    // добавляем статистику пользователю
                    await _statisticService.AddStatisticToUser((long) socketMessage.Author.Id, Statistic.Messages);
                    // проверяем выполнил ли пользователь достижение
                    await _achievementService.CheckAchievement(
                        (long) socketMessage.Author.Id, Achievement.FirstMessage);
                }
            }
        }

        private static async Task DeleteMessage(IDeletable message)
        {
            // задерживаем таск для того чтобы дискорд успел обработать сообщение
            await Task.Delay(1000);
            // удаляем сообщение
            await message.DeleteAsync();
        }

        private async Task AddVotes(IUserMessage message)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // добавляем реакции голосования
            await message.AddReactionsAsync(new IEmote[]
            {
                Emote.Parse(emotes.GetEmoteOrBlank("Like")),
                Emote.Parse(emotes.GetEmoteOrBlank("Dislike")),
            });
        }

        private static bool CheckAttachment(SocketMessage message) =>
            // убеждаемся что количество вложений равно 1
            message.Attachments.Count == 1 ||
            // или что в сообщении есть ссылка
            message.Content.Contains("http");
    }
}
