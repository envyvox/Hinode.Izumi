using System.Globalization;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;

namespace Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Impl
{
    [InjectableService]
    public class DiscordEmbedService : IDiscordEmbedService
    {
        private readonly IDiscordGuildService _discordGuildService;

        public DiscordEmbedService(IDiscordGuildService discordGuildService)
        {
            _discordGuildService = discordGuildService;
        }

        public Embed BuildEmbed(EmbedBuilder embedBuilder) =>
            embedBuilder
                // устанавливаем цвет embed-сообщения по-умолчанию
                .WithColor(new Color(uint.Parse("36393F", NumberStyles.HexNumber)))
                .Build();

        public async Task SendEmbed(SocketUser socketUser, EmbedBuilder embedBuilder, string message = "")
        {
            try
            {
                // отправляем embed-сообщение пользователю
                await socketUser.SendMessageAsync(message, false, BuildEmbed(embedBuilder));
            }
            catch
            {
                // игнорируем
            }
        }

        public async Task SendEmbed(ISocketMessageChannel socketMessageChannel, EmbedBuilder embedBuilder,
            string message = "")
        {
            try
            {
                // отправляем embed-сообщение в текстовый канал
                await socketMessageChannel.SendMessageAsync(message, false, BuildEmbed(embedBuilder));
            }
            catch
            {
                // игнорируем
            }
        }

        public async Task ModifyEmbed(IUserMessage userMessage, EmbedBuilder embedBuilder) =>
            // заменяем указанное embed-сообщение на новое
            await userMessage.ModifyAsync(x => x.Embed = BuildEmbed(embedBuilder));
    }
}
