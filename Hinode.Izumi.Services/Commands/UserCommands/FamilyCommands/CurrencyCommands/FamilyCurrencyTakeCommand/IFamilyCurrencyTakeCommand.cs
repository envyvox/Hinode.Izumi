using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.CurrencyCommands.FamilyCurrencyTakeCommand
{
    public interface IFamilyCurrencyTakeCommand
    {
        Task Execute(SocketCommandContext context, long amount, string currencyNamePattern);
    }
}