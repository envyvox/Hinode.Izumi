using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.Commands.UserCommands.ContractCommands.ContractAcceptCommand;
using Hinode.Izumi.Services.Commands.UserCommands.ContractCommands.ContractListCommand;

namespace Hinode.Izumi.Services.Commands.UserCommands.ContractCommands
{
    [CommandCategory(CommandCategory.Contract)]
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

        [Command("")]
        [Summary("Посмотреть доступные рабочие контракты в текущей локации")]
        public async Task ContractListTask() =>
            await _contractListCommand.Execute(Context);

        [Command("принять"), Alias("accept")]
        [Summary("Взяться за выполнение указанного рабочего контракта")]
        [CommandUsage("!контракт принять 1", "!контракт принять 5")]
        public async Task ContractAcceptTask(
            [Summary("Номер контракта")] long contractId) =>
            await _contractAcceptCommand.Execute(Context, contractId);
    }
}
