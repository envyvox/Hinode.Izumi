using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.EmoteService.Models;
using Hinode.Izumi.Services.RpgServices.BuildingService;
using Hinode.Izumi.Services.RpgServices.FamilyService;

namespace Hinode.Izumi.Services.Commands.UserCommands.BuildingCommands
{
    [CommandCategory(CommandCategory.Building)]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class BuildingListCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IFamilyService _familyService;
        private readonly IBuildingService _buildingService;

        private Dictionary<string, EmoteModel> _emotes;

        public BuildingListCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IFamilyService familyService, IBuildingService buildingService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _familyService = familyService;
            _buildingService = buildingService;
        }

        [Command("постройки"), Alias("buildings")]
        [Summary("Посмотреть возведенные вами или семьей постройки")]
        public async Task BuildingListTask()
        {
            // получаем иконки из базы
            _emotes = await _emoteService.GetEmotes();

            // заполняем строку персональными постройками
            var personalBuildings = await DisplayPersonalBuildings();
            // заполняем строку семейными постройками
            var familyBuildings = await DisplayFamilyBuildings();

            var embed = new EmbedBuilder()
                .WithDescription(
                    IzumiReplyMessage.BuildingListDesc.Parse() +
                    $"\n{_emotes.GetEmoteOrBlank("Blank")}")
                // персональные постройки
                .AddField($"{_emotes.GetEmoteOrBlank("List")} {BuildingCategory.Personal.Localize()}",
                    (personalBuildings.Length > 0
                        // если у пользователя есть персональные постройки - выводим их
                        ? personalBuildings
                        // если нет - предлагаем купить чертеж
                        : IzumiReplyMessage.BuildingListPersonalBuildingsNull.Parse(
                            Location.Seaport.Localize(true), _emotes.GetEmoteOrBlank("Project"))) +
                    $"{_emotes.GetEmoteOrBlank("Blank")}")
                // семейные постройки
                .AddField($"{_emotes.GetEmoteOrBlank("List")} {BuildingCategory.Family.Localize()}",
                    familyBuildings.Length > 0
                        // если у семьи пользователя есть семейные постройки - выводим их
                        ? familyBuildings
                        // если нет - предлагаем купить чертеж
                        : IzumiReplyMessage.BuildingListFamilyBuildingsNull.Parse(
                            Location.Seaport.Localize(true), _emotes.GetEmoteOrBlank("Project")));

            await _discordEmbedService.SendEmbed(Context.User, embed);
            await Task.CompletedTask;
        }

        private async Task<string> DisplayPersonalBuildings()
        {
            // получаем постройки пользователя
            var userBuildings = await _buildingService.GetUserBuildings((long) Context.User.Id);
            // возвращаем локализированную строку с информацией о постройках
            return userBuildings.Aggregate(string.Empty, (current, building) => current +
                $"\n{_emotes.GetEmoteOrBlank(building.Type.ToString())} {building.Name}\n*{building.Description}*\n");
        }

        private async Task<string> DisplayFamilyBuildings()
        {
            // проверяем состоит ли пользователь в семье
            var hasFamily = await _familyService.CheckUserHasFamily((long) Context.User.Id);
            // если пользователь не состоит в семье - предлагаем ему вступить или создать свою
            if (!hasFamily) return IzumiReplyMessage.FamilyInfoUserFamilyNull.Parse(
                Location.Capital.Localize(true));
            // получаем семью пользователя
            var userFamily = await _familyService.GetUserFamily((long) Context.User.Id);
            // получаем семейные постройки
            var familyBuildings = await _buildingService.GetFamilyBuildings(userFamily.FamilyId);
            // возвращаем локализированную строку с информацией о постройках
            return familyBuildings.Aggregate(string.Empty, (current, building) => current +
                $"\n{_emotes.GetEmoteOrBlank(building.Type.ToString())} {building.Name}\n*{building.Description}*\n");
        }
    }
}
