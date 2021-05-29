using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands
{
    [RequireContext(ContextType.DM)]
    public class HelpCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;

        public HelpCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Command("помощь"), Alias("help")]
        public async Task HelpTask([Remainder] string anyInput = null)
        {
            // получаем каналы сервера
            var channels = await _mediator.Send(new GetDiscordChannelsQuery());
            var embed = new EmbedBuilder()
                .WithDescription(
                    $"Не доступно во время раннего доступа, обращайтесь в <#{channels[DiscordChannel.Chat].Id}> по всем вопросам.");

            await _mediator.Send(new SendEmbedToUserCommand(Context.User, embed));
            await Task.CompletedTask;
        }
    }
}
