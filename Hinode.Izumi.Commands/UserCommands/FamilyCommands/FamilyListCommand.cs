using System.Linq;
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
using Hinode.Izumi.Services.GameServices.FamilyService.Queries;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands.FamilyCommands
{
    [CommandCategory(CommandCategory.Family)]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class FamilyListCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;

        public FamilyListCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Command("семьи"), Alias("families")]
        [Summary("Посмотреть список семей в игровом мире")]
        public async Task FamilyListTask()
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем все семьи
            var families = await _mediator.Send(new GetAllFamiliesQuery());

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

            await _mediator.Send(new SendEmbedToUserCommand(Context.User, embed));
            await Task.CompletedTask;
        }
    }
}
