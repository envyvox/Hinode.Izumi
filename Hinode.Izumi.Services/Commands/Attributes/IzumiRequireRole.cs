using System;
using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;
using Microsoft.Extensions.DependencyInjection;

namespace Hinode.Izumi.Services.Commands.Attributes
{
    public class IzumiRequireRole : PreconditionAttribute
    {
        private readonly DiscordRole _role;

        public IzumiRequireRole(DiscordRole role)
        {
            _role = role;
        }

        public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context,
            CommandInfo command, IServiceProvider services)
        {
            var discordGuildService = services.GetService<IDiscordGuildService>();
            // проверяем есть ли у пользователя необходимая роль
            var checkRole = await discordGuildService.CheckRoleInUser(
                (long) context.User.Id, _role);
            // проверяем есть ли у пользователя роль администратора
            var checkAdmin = await discordGuildService.CheckRoleInUser(
                (long) context.User.Id, DiscordRole.Administration);

            // возвращаем результат в зависимости от проверки
            return await Task.FromResult(checkRole
                ? PreconditionResult.FromSuccess()
                : checkAdmin
                    // роль администрации заменяет любую роль
                    ? PreconditionResult.FromSuccess()
                    : PreconditionResult.FromError(IzumiPreconditionErrorMessage.RequireRole.Parse(_role.Name())));
        }
    }
}
