using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.UserCommands.ShopCommands.FisherCommands.FisherListCommand
{
    public interface IFisherListCommand
    {
        Task Execute(SocketCommandContext context);
    }
}
