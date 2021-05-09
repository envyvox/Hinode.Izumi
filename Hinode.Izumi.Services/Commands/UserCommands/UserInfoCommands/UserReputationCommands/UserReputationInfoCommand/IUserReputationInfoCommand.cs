using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Data.Enums.ReputationEnums;

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserReputationCommands.UserReputationInfoCommand
{
    public interface IUserReputationInfoCommand
    {
        Task Execute(SocketCommandContext context, Reputation reputation);
    }
}
