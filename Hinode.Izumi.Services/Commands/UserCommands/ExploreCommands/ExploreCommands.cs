using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.Commands.UserCommands.ExploreCommands.ExploreCastleCommand;
using Hinode.Izumi.Services.Commands.UserCommands.ExploreCommands.ExploreGardenCommand;

namespace Hinode.Izumi.Services.Commands.UserCommands.ExploreCommands
{
    [Group("исследовать"), Alias("explore")]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class ExploreCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IExploreGardenCommand _exploreGardenCommand;
        private readonly IExploreCastleCommand _exploreCastleCommand;

        public ExploreCommands(IExploreGardenCommand exploreGardenCommand, IExploreCastleCommand exploreCastleCommand)
        {
            _exploreGardenCommand = exploreGardenCommand;
            _exploreCastleCommand = exploreCastleCommand;
        }

        [Command("сад"), Alias("garden")]
        [IzumiRequireLocation(Location.Garden), IzumiRequireNoDebuff(BossDebuff.GardenStop)]
        public async Task ExploreGardenTask() =>
            await _exploreGardenCommand.Execute(Context);

        [Command("шахту"), Alias("mine")]
        [IzumiRequireLocation(Location.Castle), IzumiRequireNoDebuff(BossDebuff.CastleStop)]
        public async Task ExploreCastleTask() =>
            await _exploreCastleCommand.Execute(Context);
    }
}
