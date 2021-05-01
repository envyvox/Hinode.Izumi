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
using Hinode.Izumi.Services.RpgServices.BuildingService;
using Hinode.Izumi.Services.RpgServices.ProjectService;

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands
{
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class UserProjectsCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IProjectService _projectService;
        private readonly IBuildingService _buildingService;

        public UserProjectsCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IProjectService projectService, IBuildingService buildingService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _projectService = projectService;
            _buildingService = buildingService;
        }

        [Command("чертежи"), Alias("projects")]
        public async Task UserProjectTask()
        {
            // получаем все иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем чертежи пользователя
            var userProjects = await _projectService.GetUserProject((long) Context.User.Id);

            var embed = new EmbedBuilder()
                // рассказываем как пользоваться чертежами
                .WithDescription(
                    IzumiReplyMessage.UserProjectsDesc.Parse() +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // говорим о том, что после использования чертеж будет изъят
                .WithFooter(IzumiReplyMessage.UserProjectsFooter.Parse());

            // для каждого чертежа создаем embed field
            foreach (var project in userProjects)
            {
                // получаем постройку из этого чертежа
                var building = await _buildingService.GetBuildingByProjectId(project.Id);
                // заполняем информацию о чертеже
                embed.AddField(IzumiReplyMessage.UserProjectsFieldName.Parse(
                        emotes.GetEmoteOrBlank("List"), project.Id, building.Category.Localize(true),
                        emotes.GetEmoteOrBlank("Project"), project.Name),
                    IzumiReplyMessage.UserProjectsFieldDesc.Parse(
                        emotes.GetEmoteOrBlank(building.Type.ToString()), building.Name, building.Description) +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}");
            }

            await _discordEmbedService.SendEmbed(Context.User, embed);
            await Task.CompletedTask;
        }
    }
}
