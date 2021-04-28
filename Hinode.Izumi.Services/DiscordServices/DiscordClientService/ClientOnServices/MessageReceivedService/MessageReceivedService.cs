using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.AchievementEnums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;
using Hinode.Izumi.Services.RpgServices.AchievementService;
using Hinode.Izumi.Services.RpgServices.StatisticService;
using Hinode.Izumi.Services.RpgServices.UserService;

namespace Hinode.Izumi.Services.DiscordServices.DiscordClientService.ClientOnServices.MessageReceivedService
{
    [InjectableService]
    public class MessageReceivedService : IMessageReceivedService
    {
        private readonly IDiscordGuildService _discordGuildService;
        private readonly IStatisticService _statisticService;
        private readonly IUserService _userService;
        private readonly IAchievementService _achievementService;

        public MessageReceivedService(IDiscordGuildService discordGuildService, IStatisticService statisticService,
            IUserService userService, IAchievementService achievementService)
        {
            _discordGuildService = discordGuildService;
            _statisticService = statisticService;
            _userService = userService;
            _achievementService = achievementService;
        }

        public async Task Execute(DiscordSocketClient socketClient, SocketMessage socketMessage)
        {
            // игнорируем сообщениния от ботов
            if (socketMessage.Author.IsBot) return;

            // получаем каналы сервера
            var channels = await _discordGuildService.GetChannels();
            // каналы в которых нужно добавлять реакции голосования
            var channelsToAddVotes = new List<ulong>
            {
                (ulong) channels[DiscordChannel.Suggestions].Id,
                (ulong) channels[DiscordChannel.Memes].Id,
                (ulong) channels[DiscordChannel.Arts].Id,
                (ulong) channels[DiscordChannel.Erotic].Id,
                (ulong) channels[DiscordChannel.Nsfw].Id
            };
            // каналы в которых нужно удалять сообщения без изображений
            var channelsToRemoveMessages = new List<ulong>
            {
                (ulong) channels[DiscordChannel.Memes].Id,
                (ulong) channels[DiscordChannel.Arts].Id,
                (ulong) channels[DiscordChannel.Erotic].Id,
                (ulong) channels[DiscordChannel.Nsfw].Id
            };

            // проверяем нужно ли удалить сообщение
            if (socketMessage.Attachments.Count < 1 &&
                !socketMessage.Content.Contains("http") &&
                channelsToRemoveMessages.Contains(socketMessage.Channel.Id))
                await DeleteMessage(socketMessage);

            // проверяем нужно ли добавить реакции голосования
            if (channelsToAddVotes.Contains(socketMessage.Channel.Id))
                await AddVotes((IUserMessage) socketMessage);

            // проверяем зарегистрирован ли пользователь в игровом мире
            var checkUser = await _userService.CheckUser((long) socketMessage.Author.Id);

            if (checkUser && (long) socketMessage.Channel.Id == channels[DiscordChannel.Chat].Id)
            {
                // добавляем статистику пользователю
                await _statisticService.AddStatisticToUser((long) socketMessage.Author.Id, Statistic.Messages);
                // проверяем выполнил ли пользователь достижение
                await _achievementService.CheckAchievement(
                    (long) socketMessage.Author.Id, Achievement.FirstMessage);
            }
        }

        /// <summary>
        /// Добавляет реакции голосования к сообщению.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        private static async Task AddVotes(IUserMessage message)
        {
            // добавляем массив реакций
            await message.AddReactionsAsync(new IEmote[]
            {
                new Emoji("👍"), // реакция голосования "за"
                new Emoji("👎") // реакция голосования "против"
            });
        }

        /// <summary>
        /// Удаляет сообщение.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        private static async Task DeleteMessage(IDeletable message)
        {
            // задерживаем таск для того чтобы дискорд успел обработать сообщение
            await Task.Delay(1000);
            // удаляем сообщение
            await message.DeleteAsync();
        }
    }
}
