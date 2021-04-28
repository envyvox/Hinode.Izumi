using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Hinode.Izumi.Services.DiscordServices.DiscordClientService.ClientOnServices.ReactionService
{
    public interface IReactionService
    {
        Task Execute(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction);
    }
}
