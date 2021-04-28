using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.RpgServices.FamilyService;

namespace Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.BaseCommands.FamilyCheckInfoCommand
{
    [InjectableService]
    public class FamilyCheckInfoCommand : IFamilyCheckInfoCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IFamilyService _familyService;

        public FamilyCheckInfoCommand(IDiscordEmbedService discordEmbedService, IFamilyService familyService)
        {
            _discordEmbedService = discordEmbedService;
            _familyService = familyService;
        }

        public async Task Execute(SocketCommandContext context, string familyName)
        {
            // получаем локализированное описание семье
            var embed = await _familyService.DisplayFamily(new EmbedBuilder(),
                await _familyService.GetFamily(familyName));

            await _discordEmbedService.SendEmbed(context.User, embed);
            await Task.CompletedTask;
        }
    }
}
