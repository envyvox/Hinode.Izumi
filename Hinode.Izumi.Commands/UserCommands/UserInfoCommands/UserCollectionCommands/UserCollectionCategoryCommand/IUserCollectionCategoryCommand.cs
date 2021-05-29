using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Commands.UserCommands.UserInfoCommands.UserCollectionCommands.UserCollectionCategoryCommand
{
    public interface IUserCollectionCategoryCommand
    {
        Task Execute(SocketCommandContext context, CollectionCategory category);
    }
}
