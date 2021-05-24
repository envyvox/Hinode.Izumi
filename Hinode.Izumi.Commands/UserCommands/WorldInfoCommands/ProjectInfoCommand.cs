using System.Globalization;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Commands.Attributes;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.BuildingService.Queries;
using Hinode.Izumi.Services.GameServices.IngredientService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.ProjectService.Queries;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using Humanizer;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands.WorldInfoCommands
{
    [CommandCategory(CommandCategory.Building, CommandCategory.WorldInfo)]
    [IzumiRequireRegistry]
    public class ProjectInfoCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public ProjectInfoCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        [Command("чертеж"), Alias("project")]
        [Summary("Посмотреть информацию об указанном чертеже")]
        [CommandUsage("!чертеж 1", "!чертеж 5")]
        public async Task ProjectInfoTask(
            [Summary("Номер чертежа")] long projectId)
        {
            // получаем чертеж
            var project = await _mediator.Send(new GetProjectQuery(projectId));
            // получаем постройку из этого чертежа
            var building = await _mediator.Send(new GetBuildingByProjectIdQuery(project.Id));
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем локализированную строку с ингредиентами чертежа
            var ingredients = await _mediator.Send(new DisplayProjectIngredientsQuery(project.Id));
            // считаем стоимость постройки
            var buildingCost = project.Time * await _mediator.Send(
                new GetPropertyValueQuery(Property.BuildingHourPrice));

            var requiredBuildingString = $"{emotes.GetEmoteOrBlank("Blank")} Отсутсвует";

            if (project.ReqBuildingId is not null)
            {
                var requiredBuilding = await _mediator.Send(new GetBuildingByIdQuery((long) project.ReqBuildingId));
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

            await _mediator.Send(new SendEmbedToUserCommand(Context.User, embed));
            await Task.CompletedTask;
        }
    }
}
