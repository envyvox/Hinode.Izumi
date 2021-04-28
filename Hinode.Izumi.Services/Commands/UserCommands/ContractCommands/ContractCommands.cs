using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.Commands.UserCommands.ContractCommands.ContractAcceptCommand;
using Hinode.Izumi.Services.Commands.UserCommands.ContractCommands.ContractListCommand;

namespace Hinode.Izumi.Services.Commands.UserCommands.ContractCommands
{
    [Group("контракт"), Alias("контракты", "contract", "contracts")]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class ContractCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IContractListCommand _contractListCommand;
        private readonly IContractAcceptCommand _contractAcceptCommand;

        public ContractCommands(IContractListCommand contractListCommand, IContractAcceptCommand contractAcceptCommand)
        {
            _contractListCommand = contractListCommand;
            _contractAcceptCommand = contractAcceptCommand;
        }

        [Command]
        public async Task ContractListTask() =>
            await _contractListCommand.Execute(Context);

        [Command("принять"), Alias("accept")]
        public async Task ContractAcceptTask(long contractId) =>
            await _contractAcceptCommand.Execute(Context, contractId);
    }
}
