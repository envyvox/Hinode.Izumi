using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.UserCommands.ContractCommands.ContractAcceptCommand
{
    public interface IContractAcceptCommand
    {
        Task Execute(SocketCommandContext context, long contractId);
    }
}
