using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Hinode.Izumi.Data.Enums.DiscordEnums;

namespace Hinode.Izumi.Services.DiscordServices.DiscordEmbedService
{
    public interface IDiscordEmbedService
    {
        /// <summary>
        /// Возвращает собранный embed с предустановленным цветом.
        /// </summary>
        /// <param name="embedBuilder">Embed конструктор.</param>
        /// <returns>Собранный embed с предустановленным цветом</returns>
        Embed BuildEmbed(EmbedBuilder embedBuilder);

        /// <summary>
        /// Отправляет embed-сообщение указанному пользователю.
        /// </summary>
        /// <param name="socketUser">Пользователь.</param>
        /// <param name="embedBuilder">Embed конструктор.</param>
        /// <param name="message">Сообщение перед embed (опционально).</param>
        /// <returns>Отправленное сообщение.</returns>
        Task<IUserMessage> SendEmbed(SocketUser socketUser, EmbedBuilder embedBuilder, string message = "");

        /// <summary>
        /// Отправляет embed-сообщение в указанный текстовый канал.
        /// </summary>
        /// <param name="socketMessageChannel">Текстовый канал.</param>
        /// <param name="embedBuilder">Embed конструктор.</param>
        /// <param name="message">Сообщение перед embed (опционально).</param>
        /// <returns>Отправленное сообщение.</returns>
        Task<IUserMessage> SendEmbed(ISocketMessageChannel socketMessageChannel, EmbedBuilder embedBuilder,
            string message = "");

        /// <summary>
        /// Отправляет embed-сообщение в указанный текстовый канал.
        /// </summary>
        /// <param name="channelId">ID текстового канала.</param>
        /// <param name="embedBuilder">Embed конструктор.</param>
        /// <param name="message">Сообщение перед embed (опционально).</param>
        /// <returns>Отправленное сообщение.</returns>
        Task<IUserMessage> SendEmbed(long channelId, EmbedBuilder embedBuilder, string message = "");

        /// <summary>
        /// Отправляет embed-сообщение в указанный текстовый канал.
        /// </summary>
        /// <param name="channel">Канал сервера.</param>
        /// <param name="embedBuilder">Embed конструктор.</param>
        /// <param name="message">Сообщение перед embed (опционально).</param>
        /// <returns>Отправленное сообщение.</returns>
        Task<IUserMessage> SendEmbed(DiscordChannel channel, EmbedBuilder embedBuilder, string message = "");

        /// <summary>
        /// Заменяет embed-сообщение на новое.
        /// </summary>
        /// <param name="userMessage">Сообщение.</param>
        /// <param name="embedBuilder">Embed конструктор.</param>
        Task ModifyEmbed(IUserMessage userMessage, EmbedBuilder embedBuilder);
    }
}
