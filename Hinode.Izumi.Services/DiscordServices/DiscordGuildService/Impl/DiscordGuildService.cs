using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.DiscordServices.DiscordClientService;
using Hinode.Izumi.Services.DiscordServices.DiscordClientService.Options;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Impl
{
    [InjectableService]
    public class DiscordGuildService : IDiscordGuildService
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;
        private readonly IOptions<DiscordOptions> _options;
        private readonly IDiscordClientService _discordClientService;

        private const string ChannelsKey = "channels";
        private const string RolesKey = "roles";

        public DiscordGuildService(IConnectionManager con, IMemoryCache cache, IOptions<DiscordOptions> options,
            IDiscordClientService discordClientService)
        {
            _con = con;
            _cache = cache;
            _options = options;
            _discordClientService = discordClientService;
        }

        public async Task<SocketUser> GetSocketUser(long userId) =>
            // получаем клиент дискорда
            await Task.FromResult((await _discordClientService.GetSocketClient())
                // получаем пользователя дискорда
                .GetUser((ulong) userId));

        public async Task<SocketTextChannel> GetSocketTextChannel(long channelId) =>
            // получаем сервер дискорда
            await Task.FromResult((await GetSocketGuild())
                // получаем текстовый канал этого сервера
                .GetTextChannel((ulong) channelId));

        public async Task<IUserMessage> GetIUserMessage(long channelId, long messageId) =>
            // получаем текстовый канал
            await await Task.FromResult((await GetSocketTextChannel(channelId))
                // получаем сообщение пользователя
                .GetMessageAsync((ulong) messageId)) as IUserMessage;

        public async Task<Dictionary<DiscordChannel, DiscordChannelModel>> GetChannels()
        {
            // проверяем каналы в кэше
            if (_cache.TryGetValue(ChannelsKey, out Dictionary<DiscordChannel, DiscordChannelModel> channels))
                return channels;

            // получаем каналы из базы
            channels = (await _con.GetConnection()
                    .QueryAsync<DiscordChannelModel>(@"
                        select * from discord_channels"))
                .ToDictionary(x => x.Channel);

            // получаем массив каналов которые должны быть
            var channelTypes = Enum.GetValues(typeof(DiscordChannel))
                .Cast<DiscordChannel>()
                .ToArray();

            // проверяем есть ли все каналы в библиотеке
            if (channels.Count < channelTypes.Length)
            {
                // если количество не совпадает - нужно создать и добавить недостающие каналы
                foreach (var channel in channelTypes)
                {
                    // если такой канал уже есть в библиотеке - пропускаем
                    if (channels.ContainsKey(channel)) continue;

                    // получаем сервер дискорда
                    var guild = await GetSocketGuild();
                    // ищем канал на сервере
                    var chan = guild.Channels.FirstOrDefault(x => x.Name == channel.Name());

                    // если такого канала нет - его нужно создать
                    if (chan == null)
                    {
                        // создание канала отличается в зависимости от его категории
                        switch (channel.Category())
                        {
                            case DiscordChannelCategory.TextChannel:

                                // создаем текстовый канал
                                var textChannel = await guild.CreateTextChannelAsync(channel.Name(), x =>
                                {
                                    // и указываем его родительский канал, если он имеется
                                    x.CategoryId = channels.ContainsKey(channel.Parent())
                                        ? (ulong) channels[channel.Parent()].Id
                                        : Optional<ulong?>.Unspecified;
                                });

                                // добавляем канал в библиотеку
                                channels.Add(channel, new DiscordChannelModel
                                {
                                    Id = (long) textChannel.Id,
                                    Channel = channel
                                });
                                // добавляем канал в базу
                                await AddDiscordChannel((long) textChannel.Id, channel);

                                break;
                            case DiscordChannelCategory.VoiceChannel:

                                // создаем голосовой канал
                                var voiceChannel = await guild.CreateVoiceChannelAsync(channel.Name(), x =>
                                {
                                    // и указываем его родительский канал, если он имеется
                                    x.CategoryId = channels.ContainsKey(channel.Parent())
                                        ? (ulong) channels[channel.Parent()].Id
                                        : Optional<ulong?>.Unspecified;
                                });

                                // добавляем канал в библиотеку
                                channels.Add(channel, new DiscordChannelModel
                                {
                                    Id = (long) voiceChannel.Id,
                                    Channel = channel
                                });
                                // добавляем канал в базу
                                await AddDiscordChannel((long) voiceChannel.Id, channel);

                                break;
                            case DiscordChannelCategory.CategoryChannel:

                                // создаем родительский канал
                                var categoryChannel = await guild.CreateCategoryChannelAsync(channel.Name());

                                // добавляем канал в библиотеку
                                channels.Add(channel, new DiscordChannelModel
                                {
                                    Id = (long) categoryChannel.Id,
                                    Channel = channel
                                });
                                // добавляем канал в базу
                                await AddDiscordChannel((long) categoryChannel.Id, channel);

                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    // если канал найден - его нужно просто добавить в библиотеку
                    else
                    {
                        // добавляем в библиотеку
                        channels.Add(channel, new DiscordChannelModel
                        {
                            Id = (long) chan.Id,
                            Channel = channel
                        });
                        // добавляем в базу
                        await AddDiscordChannel((long) chan.Id, channel);
                    }
                }
            }

            // добавляем каналы в кэш
            _cache.Set(ChannelsKey, channels, CacheExtensions.DefaultCacheOptions);

            // возвращаем каналы
            return channels;
        }

        public async Task<Dictionary<DiscordRole, DiscordRoleModel>> GetRoles()
        {
            // проверяем роли в кэше
            if (_cache.TryGetValue(RolesKey, out Dictionary<DiscordRole, DiscordRoleModel> roles)) return roles;

            // получаем роли из базы
            roles = (await _con.GetConnection()
                    .QueryAsync<DiscordRoleModel>(@"
                        select * from discord_roles"))
                .ToDictionary(x => x.Role);

            // получаем массив ролей которые должны быть
            var roleTypes = Enum.GetValues(typeof(DiscordRole))
                .Cast<DiscordRole>()
                .ToArray();

            // проверяем есть ли все роли в библиотеке
            if (roles.Count < roleTypes.Length)
            {
                // если количество не совпадает - нужно создать и добавить недостающие роли
                foreach (var role in roleTypes)
                {
                    // если такая роль уже есть в библиотеке - пропускаем
                    if (roles.ContainsKey(role)) continue;

                    // получаем сервер дискорда
                    var guild = await GetSocketGuild();
                    // ищем роль на сервере
                    var roleInGuild = guild.Roles.FirstOrDefault(x => x.Name == role.Name());

                    // если такой роли нет - ее нужно создать
                    if (roleInGuild == null)
                    {
                        // создаем роль
                        var newRole = await guild.CreateRoleAsync(
                            name: role.Name(),
                            permissions: null,
                            color: new Color(uint.Parse(role.Color(), NumberStyles.HexNumber)),
                            isHoisted: false,
                            options: null);

                        // добавляем роль в библиотеку
                        roles.Add(role, new DiscordRoleModel
                        {
                            Id = (long) newRole.Id,
                            Role = role
                        });
                        // добавляем роль в базу
                        await AddDiscordRole((long) newRole.Id, role);
                    }
                    // если есть - ее нужно просто добавить в библиотеку
                    else
                    {
                        // добавляем роль в библиотеку
                        roles.Add(role, new DiscordRoleModel
                        {
                            Id = (long) roleInGuild.Id,
                            Role = role
                        });
                        // добавляем роль в базу
                        await AddDiscordRole((long) roleInGuild.Id, role);
                    }
                }
            }

            // добавляем роли в кэш
            _cache.Set(RolesKey, roles, CacheExtensions.DefaultCacheOptions);

            // возвращаем роли
            return roles;
        }

        public async Task<bool> CheckRoleInUser(long userId, DiscordRole role)
        {
            // получаем пользователя сервера
            var user = await GetSocketGuildUser(userId);
            // получаем библиотеку ролей
            var roles = await GetRoles();
            // проверяем есть ли у пользователя необходимая роль
            return user.Roles.Any(x => x.Id == (ulong) roles[role].Id);
        }

        public async Task Rename(long userId, string username)
        {
            // получаем сервер дискорда
            var guild = await GetSocketGuild();
            // получаем пользователя сервера
            var user = await GetSocketGuildUser(userId);

            try
            {
                // пробуем переименовать пользователя
                await user.ModifyAsync(x => x.Nickname = username + " 🌺");
            }
            catch
            {
                // если переименовать не получилось - отправляем сообщение в администраторскую
                var channels = await GetChannels();
                await guild.GetTextChannel((ulong) channels[DiscordChannel.AdministrationChat].Id)
                    .SendMessageAsync(IzumiReplyMessage.RegistrationSuccessButCantRename.Parse(
                        user.Mention, username));
            }
        }

        public async Task ToggleRoleInUser(long userId, DiscordRole role, bool adding)
        {
            // получаем сервер дискорда
            var guild = await GetSocketGuild();
            // получаем библиотеку ролей
            var roles = await GetRoles();
            // находим нужную роль на сервере
            var socketRole = guild.GetRole((ulong) roles[role].Id);
            // получаем пользователя сервера
            var user = await GetSocketGuildUser(userId);

            // переключем роль
            await ToggleRole(user, socketRole, adding);
        }

        public async Task ToggleRoleInUser(long userId, long roleId, bool adding)
        {
            // получаем сервер дискорда
            var guild = await GetSocketGuild();
            // находим нужную роль на сервере
            var socketRole = guild.GetRole((ulong) roleId);
            // получаем пользователя сервера
            var user = await GetSocketGuildUser(userId);

            // переключем роль
            await ToggleRole(user, socketRole, adding);
        }

        public async Task MoveUserInChannel(long userId, RestVoiceChannel channel) =>
            // получаем пользователя сервера
            await (await GetSocketGuildUser(userId))
                // изменяем его текущий канал на нужный
                .ModifyAsync(x => { x.Channel = channel; });

        /// <summary>
        /// Добавляет или снимает роль у пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <param name="role">Роль.</param>
        /// <param name="adding">True если нужно добавить роль, false если снять.</param>
        private static async Task ToggleRole(IGuildUser user, IRole role, bool adding)
        {
            // если роль нужно добавить - добавляем
            if (adding) await user.AddRoleAsync(role);
            // если нет - снимаем
            else await user.RemoveRoleAsync(role);
        }

        /// <summary>
        /// Возвращает сервер дискорда.
        /// </summary>
        /// <returns>Сервер дискорда.</returns>
        private async Task<SocketGuild> GetSocketGuild() =>
            // получаем клиент дискорда
            await Task.FromResult((await _discordClientService.GetSocketClient())
                // получаем сервер дискорда
                .GetGuild(_options.Value.GuildId));

        /// <summary>
        /// Возвращает пользователя сервера дискорда.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Пользователь сервера дискорда.</returns>
        private async Task<SocketGuildUser> GetSocketGuildUser(long userId) =>
            // получаем сервер дискорда
            await Task.FromResult((await GetSocketGuild())
                // получаем пользователя сервера
                .GetUser((ulong) userId));

        /// <summary>
        /// Добавляет канал дискорда в бд.
        /// </summary>
        /// <param name="id">Id канала.</param>
        /// <param name="channel">Канал.</param>
        private async Task AddDiscordChannel(long id, DiscordChannel channel) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into discord_channels(id, channel)
                    values (@id, @channel)
                    on conflict (channel) do update
                        set id = @id,
                            updated_at = now()",
                    new {id, channel});

        /// <summary>
        /// Добавляет роль дискорда в бд.
        /// </summary>
        /// <param name="id">Id роли.</param>
        /// <param name="role">Роль.</param>
        private async Task AddDiscordRole(long id, DiscordRole role) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into discord_roles(id, role)
                    values (@id, @role)
                    on conflict (role) do update
                        set id = @id,
                            updated_at = now()",
                    new {id, role});
    }
}
