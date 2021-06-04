using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.BuildingService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.ProjectService.Queries;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.ShopCommands.ListCommands.Impl
{
    [InjectableService]
    public class ShopProjectCommand : IShopProjectCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public ShopProjectCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context)
        {
            var emotes = await _mediator.Send(new GetEmotesQuery());
            var projects = await _mediator.Send(new GetAllProjectsQuery());

            var embed = new EmbedBuilder()
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.ShopProject)))
                .WithDescription(
                    IzumiReplyMessage.ShopProjectDesc.Parse() +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}");

            foreach (var project in projects)
            {
                var building = await _mediator.Send(new GetBuildingByProjectIdQuery(project.Id));
                var hasProject = await _mediator.Send(new CheckUserHasProjectQuery(
                    (long) context.User.Id, project.Id));
                var hasBuilding = await _mediator.Send(new CheckBuildingInUserQuery(
                    (long) context.User.Id, building.Type));

                if (hasProject || hasBuilding) continue;

                embed.AddField(
                    $"{emotes.GetEmoteOrBlank("List")} `{project.Id}` {emotes.GetEmoteOrBlank("Project")} {project.Name}",
                    $"Стоимость: {emotes.GetEmoteOrBlank(Currency.Ien.ToString())} {project.Price} {_local.Localize(Currency.Ien.ToString(), project.Price)}");
            }

            if (embed.Fields.Count < 1)
                embed.AddField(IzumiReplyMessage.ShopProjectSoldFieldName.Parse(),
                    IzumiReplyMessage.ShopProjectSoldFieldDesc.Parse());

            await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
            await Task.CompletedTask;
        }
    }
}
