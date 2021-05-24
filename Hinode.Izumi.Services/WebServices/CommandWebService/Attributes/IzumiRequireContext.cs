using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;

namespace Hinode.Izumi.Commands.Attributes
{
    public class IzumiRequireContext : PreconditionAttribute
    {
        private readonly DiscordContext _context;

        public IzumiRequireContext(DiscordContext context)
        {
            _context = context;
        }

        public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context,
            CommandInfo command, IServiceProvider services)
        {
            // проверяем контекст команды
            var flag = _context switch
            {
                DiscordContext.Guild => context.Channel is IGuildChannel,
                DiscordContext.DirectMessage => context.Channel is IDMChannel,
                DiscordContext.Group => context.Channel is IGroupChannel,
                _ => false
            };
            // возвращаем результат в зависимости от проверки
            return await Task.FromResult(flag
                ? PreconditionResult.FromSuccess()
                : PreconditionResult.FromError(IzumiPreconditionErrorMessage.RequireContext.Parse(
                    _context.Localize())));
        }
    }
}
