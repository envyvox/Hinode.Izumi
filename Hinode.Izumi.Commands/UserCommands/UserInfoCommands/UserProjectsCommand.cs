using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Commands.Attributes;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.BuildingService.Queries;
using Hinode.Izumi.Services.GameServices.ProjectService.Queries;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands.UserInfoCommands
{
    [CommandCategory(CommandCategory.UserInfo)]
    [IzumiRequireRegistry]
    public class UserProjectsCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;

        public UserProjectsCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Command("чертежи"), Alias("projects")]
        [Summary("Посмотреть приобретенные чертежи")]
        public async Task UserProjectTask()
        {
            // получаем все иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем чертежи пользователя
            var userProjects = await _mediator.Send(new GetUserProjectsQuery((long) Context.User.Id));

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
                var building = await _mediator.Send(new GetBuildingByProjectIdQuery(project.Id));
                // заполняем информацию о чертеже
                embed.AddField(IzumiReplyMessage.UserProjectsFieldName.Parse(
                        emotes.GetEmoteOrBlank("List"), project.Id, building.Category.Localize(true),
                        emotes.GetEmoteOrBlank("Project"), project.Name),
                    IzumiReplyMessage.UserProjectsFieldDesc.Parse(
                        emotes.GetEmoteOrBlank(building.Type.ToString()), building.Name, building.Description) +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}");
            }

            await _mediator.Send(new SendEmbedToUserCommand(Context.User, embed));
            await Task.CompletedTask;
        }
    }
}
