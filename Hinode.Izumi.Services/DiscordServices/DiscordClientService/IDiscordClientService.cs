using System.Threading.Tasks;
using Discord.WebSocket;

namespace Hinode.Izumi.Services.DiscordServices.DiscordClientService
{
    public interface IDiscordClientService
    {
        Task Start();

        /// <summary>
        /// Возвращает клиент дискорда.
        /// </summary>
        /// <returns>Клиент дискорда.</returns>
        Task<DiscordSocketClient> GetSocketClient();
    }
}
