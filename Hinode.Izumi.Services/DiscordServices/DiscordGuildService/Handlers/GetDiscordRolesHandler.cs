using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Discord;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Records;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;


namespace Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Handlers
{
    public class GetDiscordRolesHandler
        : IRequestHandler<GetDiscordRolesQuery, Dictionary<DiscordRole, DiscordRoleRecord>>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;
        private readonly IMediator _mediator;

        private const string DiscordRolesKey = "discord_roles";

        public GetDiscordRolesHandler(IConnectionManager con, IMemoryCache cache, IMediator mediator)
        {
            _con = con;
            _cache = cache;
            _mediator = mediator;
        }

        public async Task<Dictionary<DiscordRole, DiscordRoleRecord>> Handle(GetDiscordRolesQuery request,
            CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(DiscordRolesKey, out Dictionary<DiscordRole, DiscordRoleRecord> roles)) return roles;

            roles = (await _con.GetConnection()
                    .QueryAsync<DiscordRoleRecord>(@"
                        select * from discord_roles"))
                .ToDictionary(x => x.Role);

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
                    var guild = await _mediator.Send(new GetDiscordSocketGuildQuery(), cancellationToken);
                    // ищем роль на сервере
                    var roleInGuild = guild.Roles.FirstOrDefault(x => x.Name == role.Name());

                    // если такой роли нет - ее нужно создать
                    if (roleInGuild is null)
                    {
                        // создаем роль
                        var newRole = await guild.CreateRoleAsync(
                            name: role.Name(),
                            permissions: null,
                            color: new Color(uint.Parse(role.Color(), NumberStyles.HexNumber)),
                            isHoisted: false,
                            options: null);

                        // добавляем роль в библиотеку
                        roles.Add(role, new DiscordRoleRecord((long) newRole.Id, role));
                        // добавляем роль в базу
                        await AddDiscordRole((long) newRole.Id, role);
                    }
                    // если есть - ее нужно просто добавить в библиотеку
                    else
                    {
                        // добавляем роль в библиотеку
                        roles.Add(role, new DiscordRoleRecord((long) roleInGuild.Id, role));
                        // добавляем роль в базу
                        await AddDiscordRole((long) roleInGuild.Id, role);
                    }
                }
            }

            _cache.Set(DiscordRolesKey, roles, CacheExtensions.DefaultCacheOptions);

            return roles;
        }

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
