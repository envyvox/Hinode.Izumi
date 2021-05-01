using System.Threading.Tasks;
using Discord;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;

namespace Hinode.Izumi.Services.BackgroundJobs.MuteJob
{
    [InjectableService]
    public class MuteJob : IMuteJob
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IDiscordGuildService _discordGuildService;

        public MuteJob(IDiscordEmbedService discordEmbedService, IDiscordGuildService discordGuildService)
        {
            _discordEmbedService = discordEmbedService;
            _discordGuildService = discordGuildService;
        }

        public async Task Unmute(long userId)
        {
            // снимаем роль блокировки чата с пользователя
            await _discordGuildService.ToggleRoleInUser(userId, DiscordRole.Mute, false);

            var embed = new EmbedBuilder()
                // подтвержаем снятия блокировки чата
                .WithDescription(IzumiReplyMessage.UnmuteDesc.Parse());

            await _discordEmbedService.SendEmbed(
                await _discordGuildService.GetSocketUser(userId), embed);
        }
    }
}
