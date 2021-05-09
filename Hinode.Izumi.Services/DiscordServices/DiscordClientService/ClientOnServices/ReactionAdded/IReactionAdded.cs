using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Hinode.Izumi.Services.DiscordServices.DiscordClientService.ClientOnServices.ReactionAdded
{
    public interface IReactionAdded
    {
        Task Execute(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel socketMessageChannel,
            SocketReaction socketReaction);
    }
}
