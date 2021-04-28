using System;

namespace Hinode.Izumi.Data.Enums.DiscordEnums
{
    /// <summary>
    /// Контекст (где проиходит действие).
    /// </summary>
    public enum DiscordContext
    {
        /// <summary>
        /// Сервер.
        /// </summary>
        Guild = 1,

        /// <summary>
        /// Личное сообщение.
        /// </summary>
        DirectMessage = 2,

        /// <summary>
        /// Группа в личных сообщениях.
        /// </summary>
        Group = 3
    }

    public static class IzumiDiscordContextHelper
    {
        /// <summary>
        /// Возвращает локализированное название контекста.
        /// </summary>
        /// <param name="context">Контекст (где проиходит действие).</param>
        /// <returns>Локализированное название контекста.</returns>
        public static string Localize(this DiscordContext context) => context switch
        {
            // выводится в сообщениях об ошибке, необходимо склонение "в *контекст*"
            DiscordContext.Guild => "канале сервера",
            DiscordContext.DirectMessage => "личном сообщении",
            DiscordContext.Group => "беседе",
            _ => throw new ArgumentOutOfRangeException(nameof(context), context, null)
        };
    }
}
