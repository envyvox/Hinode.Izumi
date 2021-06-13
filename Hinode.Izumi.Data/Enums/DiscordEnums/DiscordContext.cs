using System;

namespace Hinode.Izumi.Data.Enums.DiscordEnums
{
    public enum DiscordContext : byte
    {
        Guild = 1,
        DirectMessage = 2,
        Group = 3
    }

    public static class IzumiDiscordContextHelper
    {
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
