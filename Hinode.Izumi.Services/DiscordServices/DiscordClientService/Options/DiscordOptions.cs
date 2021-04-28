namespace Hinode.Izumi.Services.DiscordServices.DiscordClientService.Options
{
    public class DiscordOptions
    {
        /// <summary>
        /// Токен бота.
        /// </summary>
        public string BotToken { get; set; }

        /// <summary>
        /// Id дискорд-сервера.
        /// </summary>
        public ulong GuildId { get; set; }

        /// <summary>
        /// Id бота.
        /// </summary>
        public ulong BotId { get; set; }
    }
}
