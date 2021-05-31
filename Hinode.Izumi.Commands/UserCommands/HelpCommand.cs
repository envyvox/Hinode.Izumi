using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.WebServices.CommandWebService.Attributes;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands
{
    [CommandCategory(CommandCategory.WorldInfo)]
    public class HelpCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;

        public HelpCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Command("помощь"), Alias("help")]
        [Summary("Помощь по серверу")]
        public async Task HelpTask([Remainder] string anyInput = null)
        {
            var emotes = await _mediator.Send(new GetEmotesQuery());
            var channels = await _mediator.Send(new GetDiscordChannelsQuery());

            var embed = new EmbedBuilder()
                .AddField(IzumiReplyMessage.HelpGameCommandsFieldName.Parse(emotes.GetEmoteOrBlank("List")),
                    IzumiReplyMessage.HelpGameCommandsFieldDesc.Parse() +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                .AddField(IzumiReplyMessage.HelpGameMechanicsFieldName.Parse(emotes.GetEmoteOrBlank("List")),
                    IzumiReplyMessage.HelpGameMechanicsFieldDesc.Parse(channels[DiscordChannel.GameMechanics].Id) +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                .AddField(IzumiReplyMessage.HelpSuggestionsFieldName.Parse(emotes.GetEmoteOrBlank("List")),
                    IzumiReplyMessage.HelpSuggestionsFieldDesc.Parse(channels[DiscordChannel.Suggestions].Id) +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                .AddField(IzumiReplyMessage.HelpBugsFieldName.Parse(emotes.GetEmoteOrBlank("List")),
                    IzumiReplyMessage.HelpBugsFieldDesc.Parse(emotes.GetEmoteOrBlank(Box.Capital.Emote())) +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                .AddField(IzumiReplyMessage.HelpDonationFieldName.Parse(emotes.GetEmoteOrBlank("Like")),
                    IzumiReplyMessage.HelpDonationFieldDesc.Parse(emotes.GetEmoteOrBlank("Premium")));

            await _mediator.Send(new SendEmbedToUserCommand(Context.User, embed));
            await Task.CompletedTask;
        }
    }
}
