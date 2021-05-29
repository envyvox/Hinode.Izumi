using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordClientService.ClientOnServices.UserVoiceStateUpdated
{
    [InjectableService]
    public class UserVoiceStateUpdated : IUserVoiceStateUpdated
    {
        private readonly IMediator _mediator;

        public UserVoiceStateUpdated(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Execute(SocketUser user, SocketVoiceState userOldState, SocketVoiceState userNewState)
        {
            // получаем каналы сервера
            var channels = await _mediator.Send(new GetDiscordChannelsQuery());
            // получаем роли сервера
            var roles = await _mediator.Send(new GetDiscordRolesQuery());

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

            // когда пользователь заходит в голосовой канал
            if (oldChannel is null)
                // добавляем ему роль
                await _mediator.Send(new AddDiscordRoleToUserCommand((long) user.Id, DiscordRole.InVoice));

            // когда пользователь выходит из голосового канала
            if (newChannel is null)
                // забираем роль
                await _mediator.Send(new RemoveDiscordRoleFromUserCommand((long) user.Id, DiscordRole.InVoice));

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
                await _mediator.Send(new MoveDiscordUserInChannelCommand((long) user.Id, createdChannel));
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
