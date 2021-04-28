using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Framework.Autofac;

namespace Hinode.Izumi.Services.Commands.UserCommands.ShopCommands.BuyCommands.Impl
{
    [InjectableService]
    public class ShopBuyProjectCommand : IShopBuyProjectCommand
    {
        public async Task Execute(SocketCommandContext context, long projectId)
        {
            throw new System.NotImplementedException();
        }
    }
}
