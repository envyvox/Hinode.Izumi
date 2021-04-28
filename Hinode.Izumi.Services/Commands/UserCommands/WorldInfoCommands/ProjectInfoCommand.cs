using System.Globalization;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.BuildingService;
using Hinode.Izumi.Services.RpgServices.IngredientService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.ProjectService;
using Hinode.Izumi.Services.RpgServices.PropertyService;
using Humanizer;

namespace Hinode.Izumi.Services.Commands.UserCommands.WorldInfoCommands
{
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class ProjectInfoCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly ILocalizationService _local;
        private readonly IIngredientService _ingredientService;
        private readonly IProjectService _projectService;
        private readonly IBuildingService _buildingService;
        private readonly IPropertyService _propertyService;

        public ProjectInfoCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            ILocalizationService local, IIngredientService ingredientService, IProjectService projectService,
            IBuildingService buildingService, IPropertyService propertyService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _local = local;
            _ingredientService = ingredientService;
            _projectService = projectService;
            _buildingService = buildingService;
            _propertyService = propertyService;
        }

        [Command("чертеж"), Alias("project")]
        public async Task ProjectInfoTask(long projectId)
        {
            // получаем чертеж
            var project = await _projectService.GetProject(projectId);
            // получаем постройку из этого чертежа
            var building = await _buildingService.GetBuildingByProjectId(project.Id);
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем локализированную строку с ингредиентами чертежа
            var ingredients = await _ingredientService.DisplayProjectIngredients(project.Id);
            // считаем стоимость постройки
            var buildingCost = project.Time * await _propertyService.GetPropertyValue(Property.BuildingHourPrice);

            var requiredBuildingString = $"{emotes.GetEmoteOrBlank("Blank")} Отсутсвует";

            if (project.ReqBuildingId != null)
            {
                var requiredBuilding = await _buildingService.GetBuilding((long) project.ReqBuildingId);
                requiredBuildingString =
                    $"{emotes.GetEmoteOrBlank(requiredBuilding.Type.ToString())} {requiredBuilding.Name}";
            }

            var embed = new EmbedBuilder()
                // номер и название чертежа
                .WithTitle(IzumiReplyMessage.ProjectInfoTitle.Parse(
                    project.Id, building.Category.Localize(true),
                    emotes.GetEmoteOrBlank("Project"), project.Name))
                // описание
                .WithDescription(
                    IzumiReplyMessage.ProjectInfoDesc.Parse(
                        emotes.GetEmoteOrBlank(building.Type.ToString()), building.Name, building.Description) +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // требование постройки
                .AddField(IzumiReplyMessage.ProjectInfoRequireFieldName.Parse(), requiredBuildingString)
                // необходимые ресурсы
                .AddField(IzumiReplyMessage.ProjectInfoIngredientsFieldName.Parse(),
                    ingredients.Length > 0
                        ? ingredients
                        // если список ингредиентов пустая, пишем что они не требуются
                        : IzumiReplyMessage.UserProjectsIngredientsNull.Parse())
                // стоимость постройки
                .AddField(IzumiReplyMessage.ProjectInfoBuildingCostFieldName.Parse(),
                    $"{emotes.GetEmoteOrBlank(Currency.Ien.ToString())} {buildingCost} {_local.Localize(Currency.Ien.ToString(), buildingCost)}",
                    true)
                // длительность
                .AddField(IzumiReplyMessage.TimeFieldName.Parse(),
                    project.Time.Hours().Humanize(2, new CultureInfo("ru-RU")), true);

            await _discordEmbedService.SendEmbed(Context.User, embed);
            await Task.CompletedTask;
        }
    }
}
