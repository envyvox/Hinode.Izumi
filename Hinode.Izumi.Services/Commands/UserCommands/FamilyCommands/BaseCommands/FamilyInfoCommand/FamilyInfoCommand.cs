using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.RpgServices.FamilyService;

namespace Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.BaseCommands.FamilyInfoCommand
{
    [InjectableService]
    public class FamilyInfoCommand : IFamilyInfoCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IFamilyService _familyService;

        public FamilyInfoCommand(IDiscordEmbedService discordEmbedService, IFamilyService familyService)
        {
            _discordEmbedService = discordEmbedService;
            _familyService = familyService;
        }

        public async Task Execute(SocketCommandContext context)
        {
            var embed = new EmbedBuilder();

            // проверяем состоит ли пользователь в семье
            var hasFamily = await _familyService.CheckUserHasFamily((long) context.User.Id);

            // если у пользователя есть семья, выводим о ней информацию
            if (hasFamily)
            {
                // получаем пользователя в семье
                var userFamily = await _familyService.GetUserFamily((long) context.User.Id);
                // получаем семью пользователя
                var family = await _familyService.GetFamily(userFamily.FamilyId);
                // получаем локализированную информацию о семье
                embed = await _familyService.DisplayFamily(embed, family);
            }
            // если у пользователя нет семьи, рассказываем как в нее вступить или создать
            else
            {
                embed.WithDescription(IzumiReplyMessage.FamilyInfoUserFamilyNull.Parse(
                    Location.Capital.Localize(true)));
            }

            await _discordEmbedService.SendEmbed(context.User, embed);
            await Task.CompletedTask;
        }
    }
}
