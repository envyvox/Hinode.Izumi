using System.Threading.Tasks;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;
using Hinode.Izumi.Services.DiscordServices.DiscordRoleService;

namespace Hinode.Izumi.Services.BackgroundJobs.DiscordJob
{
    [InjectableService]
    public class DiscordJob : IDiscordJob
    {
        private readonly IDiscordGuildService _discordGuildService;
        private readonly IDiscordRoleService _discordRoleService;

        public DiscordJob(IDiscordGuildService discordGuildService, IDiscordRoleService discordRoleService)
        {
            _discordGuildService = discordGuildService;
            _discordRoleService = discordRoleService;
        }

        public async Task DeleteMessage(long channelId, long messageId)
        {
            // получаем сообщение
            var message = await _discordGuildService.GetIUserMessage(channelId, messageId);
            // удаляем его
            await message.DeleteAsync();
        }

        public async Task RemoveExpiredRoleFromUsers()
        {
            // получаем роли пользователей с истекшим сроком
            var expiredRoles = await _discordRoleService.GetExpiredUserRoles();

            // если такое роли есть
            if (expiredRoles.Length > 0)
            {
                // снимаем каждую из них
                foreach (var expiredRole in expiredRoles)
                    await _discordGuildService.ToggleRoleInUser(expiredRole.UserId, expiredRole.RoleId, false);
            }
        }
    }
}
