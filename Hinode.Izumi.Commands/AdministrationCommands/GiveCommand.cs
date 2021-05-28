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
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using Hinode.Izumi.Services.GameServices.InventoryService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.UserService.Queries;
using Hinode.Izumi.Services.WebServices.CommandWebService.Attributes;
using MediatR;

namespace Hinode.Izumi.Commands.AdministrationCommands
{
    [IzumiRequireRole(DiscordRole.Administration)]
    public class GiveCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public GiveCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        [Command("give")]
        public async Task GiveCommandTask(long userId, InventoryCategory category, long itemId, long amount) =>
            await GiveTask(userId, category, itemId, amount);

        [Command("give")]
        public async Task GiveCommandTask(IUser user, InventoryCategory category, long itemId, long amount) =>
            await GiveTask((long) user.Id, category, itemId, amount);

        private async Task GiveTask(long userId, InventoryCategory category, long itemId, long amount)
        {
            var emotes = await _mediator.Send(new GetEmotesQuery());
            var user = await _mediator.Send(new GetUserByIdQuery(userId));

            await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(user.Id, category, itemId, amount));

            var itemName = await _mediator.Send(new GetItemNameQuery(category, itemId));
            var emoteName = category == InventoryCategory.Box ? ((Box) itemId).Emote() : itemName;

            var embed = new EmbedBuilder()
                .WithDescription(IzumiReplyMessage.AdmGiveDesc.Parse(
                    emotes.GetEmoteOrBlank(user.Title.Emote()), user.Title.Localize(), user.Name,
                    emotes.GetEmoteOrBlank(emoteName), amount, _local.Localize(itemName, amount)));

            await _mediator.Send(new SendEmbedToUserCommand(Context.User, embed));

            var embedPm = new EmbedBuilder()
                .WithDescription(IzumiReplyMessage.AdmGivePmDesc.Parse(
                    emotes.GetEmoteOrBlank(emoteName), amount, _local.Localize(itemName, amount)));

            await _mediator.Send(new SendEmbedToUserCommand(
                await _mediator.Send(new GetDiscordSocketUserQuery(user.Id)), embedPm));
        }
    }
}
