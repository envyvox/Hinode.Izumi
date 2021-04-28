using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;

namespace Hinode.Izumi.Services.Commands.UserCommands
{
    [RequireContext(ContextType.DM)]
    public class HelpCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IDiscordGuildService _discordGuildService;

        public HelpCommand(IDiscordEmbedService discordEmbedService, IDiscordGuildService discordGuildService)
        {
            _discordEmbedService = discordEmbedService;
            _discordGuildService = discordGuildService;
        }

        [Command("помощь"), Alias("help")]
        public async Task HelpTask([Remainder] string anyInput = null)
        {
            // получаем каналы сервера
            var channels = await _discordGuildService.GetChannels();
            var embed = new EmbedBuilder()
                .WithDescription(
                    $"Не доступно во время раннего доступа, обращайтесь в <#{channels[DiscordChannel.Chat].Id}> по всем вопросам.");

            await _discordEmbedService.SendEmbed(Context.User, embed);
            await Task.CompletedTask;
        }
    }
}
