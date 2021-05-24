using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.GameServices.FamilyService.Queries;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands.FamilyCommands.BaseCommands.FamilyInfoCommand
{
    [InjectableService]
    public class FamilyInfoCommand : IFamilyInfoCommand
    {
        private readonly IMediator _mediator;

        public FamilyInfoCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Execute(SocketCommandContext context)
        {
            var embed = new EmbedBuilder();

            // проверяем состоит ли пользователь в семье
            var hasFamily = await _mediator.Send(new CheckUserHasFamilyQuery((long) context.User.Id));

            // если у пользователя есть семья, выводим о ней информацию
            if (hasFamily)
            {
                // получаем пользователя в семье
                var userFamily = await _mediator.Send(new GetUserFamilyQuery((long) context.User.Id));
                // получаем семью пользователя
                var family = await _mediator.Send(new GetFamilyByIdQuery(userFamily.FamilyId));
                // получаем локализированную информацию о семье
                embed = await _mediator.Send(new GetFamilyEmbedViewQuery(embed, family));
            }
            // если у пользователя нет семьи, рассказываем как в нее вступить или создать
            else
            {
                embed.WithDescription(IzumiReplyMessage.FamilyInfoUserFamilyNull.Parse(
                    Location.Capital.Localize(true)));
            }

            await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
            await Task.CompletedTask;
        }
    }
}
