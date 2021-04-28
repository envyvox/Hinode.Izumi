using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;

namespace Hinode.Izumi.Services.DiscordServices.DiscordClientService.ClientOnServices.UserVoiceStateUpdated
{
    [InjectableService]
    public class UserVoiceStateUpdatedService : IUserVoiceStateUpdatedService
    {
        private readonly IDiscordGuildService _discordGuildService;

        public UserVoiceStateUpdatedService(IDiscordGuildService discordGuildService)
        {
            _discordGuildService = discordGuildService;
        }

        public async Task Execute(SocketUser user, SocketVoiceState userOldState, SocketVoiceState userNewState)
        {
            // получаем каналы сервера
            var channels = await _discordGuildService.GetChannels();
            // получаем роли сервера
            var roles = await _discordGuildService.GetRoles();

            // категория для создания каналов
            var createRoomParent = (ulong) channels[DiscordChannel.CreateRoomParent].Id;
            // комната для создания каналов
            var createRoom = (ulong) channels[DiscordChannel.CreateRoom].Id;
            // роль музыкальных ботов
            var musicBot = (ulong) roles[DiscordRole.MusicBot].Id;

            // получаем старый канал в котором находился пользователь
            var oldChannel = userOldState.VoiceChannel;
            // получаем новый канал в котором находился пользователь
            var newChannel = userNewState.VoiceChannel;

            // если новый канал это комната для создания каналов
            if (newChannel?.Id == createRoom)
            {
                // создаем новый канал
                var createdChannel = await newChannel.Guild.CreateVoiceChannelAsync(user.Username, x =>
                {
                    // в категории для создания каналов
                    x.CategoryId = createRoomParent;
                    // с лимитом пользователей по-умолчанию
                    x.UserLimit = 5;
                });

                // перемещаем пользователя в созданный канал
                await _discordGuildService.MoveUserInChannel((long) user.Id, createdChannel);
                // добавляем этому каналу разрешения для пользователя
                await createdChannel.AddPermissionOverwriteAsync(
                    // пользователь
                    user,
                    // разрешаем управлять этим каналом
                    new OverwritePermissions(manageChannel: PermValue.Allow));
                // добавляем этому каналу разрешения для роли музыкального бота
                await createdChannel.AddPermissionOverwriteAsync(
                    // получаем роли музыкальных ботов
                    newChannel.Guild.GetRole(musicBot),
                    // разрашем музыкальным ботам перемещать пользователей
                    // это даст боту возможность заходить в переполненный канал
                    new OverwritePermissions(moveMembers: PermValue.Allow));
            }

            // если старый канал пользователя находится в категории для создания каналов
            if (oldChannel?.CategoryId == createRoomParent &&
                // и если количество пользователей в этом канале теперь 0
                oldChannel.Users.Count == 0 &&
                // и если этот канал не комната для создания каналов
                oldChannel.Id != createRoom)
            {
                // удаляем канал
                await oldChannel.DeleteAsync();
            }
        }
    }
}
