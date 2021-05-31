using System;
using System.Globalization;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using Hinode.Izumi.Services.DiscordServices.DiscordRoleService.Queries;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.PremiumService.Commands;
using Hinode.Izumi.Services.GameServices.PremiumService.Queries;
using Hinode.Izumi.Services.ImageService.Queries;
using Hinode.Izumi.Services.WebServices.CommandWebService.Attributes;
using Humanizer;
using Humanizer.Localisation;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.PremiumCommands
{
    [Group("премиум")]
    [RequirePremium]
    [CommandCategory(CommandCategory.Premium)]
    public class PremiumCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;

        public PremiumCommands(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Command]
        public async Task PremiumInfoTask()
        {
            var emotes = await _mediator.Send(new GetEmotesQuery());
            var roles = await _mediator.Send(new GetDiscordRolesQuery());
            var userPremium = await _mediator.Send(new GetDiscordUserRoleQuery(
                (long) Context.User.Id, roles[DiscordRole.Premium].Id));
            var userCommandColor = await _mediator.Send(new GetUserCommandColorQuery((long) Context.User.Id));

            var embed = new EmbedBuilder()
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.ColorPicker)))
                .AddField(IzumiReplyMessage.PremiumInfoChangeColorFieldName.Parse(emotes.GetEmoteOrBlank("List")),
                    IzumiReplyMessage.PremiumInfoChangeColorFieldDesc.Parse(userCommandColor) +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                .AddField(IzumiReplyMessage.PremiumInfoExpirationFieldName.Parse(emotes.GetEmoteOrBlank("List")),
                    IzumiReplyMessage.PremiumInfoExpirationFieldDesc.Parse(
                        emotes.GetEmoteOrBlank("Premium"),
                        (DateTimeOffset.Now - userPremium.Expiration).TotalDays.Days()
                        .Humanize(1, new CultureInfo("ru-RU"), TimeUnit.Day)));

            await _mediator.Send(new SendEmbedToUserCommand(Context.User, embed));
            await Task.CompletedTask;
        }

        [Command("цвет")]
        [Summary("Изменение цвета команд")]
        [CommandUsage("!премиум цвет fcba03", "!премиум цвет #c691ff")]
        public async Task PremiumColorTask(
            [Summary("HEX-цвет")] string newColor)
        {
            if (newColor.StartsWith("#")) newColor = newColor.Remove(0, 1);

            await _mediator.Send(new UpdateUserCommandColorCommand((long) Context.User.Id, newColor));

            var embed = new EmbedBuilder()
                .WithDescription(IzumiReplyMessage.PremiumChangeColor.Parse(newColor));

            await _mediator.Send(new SendEmbedToUserCommand(Context.User, embed));
            await Task.CompletedTask;
        }
    }
}
