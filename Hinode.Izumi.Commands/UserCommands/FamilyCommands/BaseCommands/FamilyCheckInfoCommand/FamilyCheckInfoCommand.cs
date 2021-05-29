using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.GameServices.FamilyService.Queries;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands.FamilyCommands.BaseCommands.FamilyCheckInfoCommand
{
    [InjectableService]
    public class FamilyCheckInfoCommand : IFamilyCheckInfoCommand
    {
        private readonly IMediator _mediator;

        public FamilyCheckInfoCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Execute(SocketCommandContext context, string familyName)
        {
            // получаем локализированное описание семьи
            var embed = await _mediator.Send(new GetFamilyEmbedViewQuery(new EmbedBuilder(),
                await _mediator.Send(new GetFamilyByNameQuery(familyName))));

            await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
            await Task.CompletedTask;
        }
    }
}
