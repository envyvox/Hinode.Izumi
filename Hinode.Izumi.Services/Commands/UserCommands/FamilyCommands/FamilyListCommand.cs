using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.FamilyService;

namespace Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands
{
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class FamilyListCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IFamilyService _familyService;

        public FamilyListCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IFamilyService familyService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _familyService = familyService;
        }

        [Command("семьи"), Alias("families")]
        public async Task FamilyListTask()
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем все семьи
            var families = await _familyService.GetAllFamilies();

            // заполняем строку названиями семей
            var familyString = families.Aggregate(string.Empty,
                (current, family) =>
                    current + $"{emotes.GetEmoteOrBlank("List")} {family.Name}\n");

            var embed = new EmbedBuilder()
                // рассказываем как посмотреть информацию о семье
                .WithDescription(IzumiReplyMessage.FamilyListDesc.Parse())
                // выводим список семей
                .AddField(IzumiReplyMessage.FamilyListFieldName.Parse(),
                    familyString.Length > 0
                        ? familyString
                        : IzumiReplyMessage.FamilyListNull.Parse());

            await _discordEmbedService.SendEmbed(Context.User, embed);
            await Task.CompletedTask;
        }
    }
}
