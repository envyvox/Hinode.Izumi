using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.ManageCommands.FamilyUpdateDescriptionCommand
{
    public interface IFamilyUpdateDescriptionCommand
    {
        Task Execute(SocketCommandContext context, string newDescription);
    }
}