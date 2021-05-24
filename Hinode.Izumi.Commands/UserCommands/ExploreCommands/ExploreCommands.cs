using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Commands.Attributes;
using Hinode.Izumi.Commands.UserCommands.ExploreCommands.ExploreCastleCommand;
using Hinode.Izumi.Commands.UserCommands.ExploreCommands.ExploreGardenCommand;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;

namespace Hinode.Izumi.Commands.UserCommands.ExploreCommands
{
    [CommandCategory(CommandCategory.Explore)]
    [Group("исследовать"), Alias("explore")]
    [IzumiRequireRegistry]
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
        [Summary("Отправиться исследовать сад")]
        [IzumiRequireLocation(Location.Garden), IzumiRequireNoDebuff(BossDebuff.GardenStop)]
        public async Task ExploreGardenTask() =>
            await _exploreGardenCommand.Execute(Context);

        [Command("шахту"), Alias("mine")]
        [Summary("Отправиться исследовать шахту")]
        [IzumiRequireLocation(Location.Castle), IzumiRequireNoDebuff(BossDebuff.CastleStop)]
        public async Task ExploreCastleTask() =>
            await _exploreCastleCommand.Execute(Context);
    }
}
