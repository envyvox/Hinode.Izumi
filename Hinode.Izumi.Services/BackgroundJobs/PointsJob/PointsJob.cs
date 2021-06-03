using System.Threading.Tasks;
using Dapper;
using Discord;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.UserService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.BackgroundJobs.PointsJob
{
    [InjectableService]
    public class PointsJob : IPointsJob
    {
        private readonly IConnectionManager _con;
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public PointsJob(IConnectionManager con, IMediator mediator, ILocalizationService local)
        {
            _con = con;
            _mediator = mediator;
            _local = local;
        }

        public async Task SendAwardsAndResetPoints()
        {
            var emotes = await _mediator.Send(new GetEmotesQuery());
            var topUsers = await _mediator.Send(new GetTopUsersQuery());

            foreach (var user in topUsers)
            {
                var displayRowNumber = await _mediator.Send(new DisplayRowNumberQuery(user.RowNumber));
                long boxAmount = user.RowNumber switch {
                    1 => 5,
                    2 => 3,
                    3 => 2,
                    _ => 1
                };

                await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                    user.Id, InventoryCategory.Box, Box.Capital.GetHashCode(), boxAmount));
                await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                    user.Id, InventoryCategory.Box, Box.Garden.GetHashCode(), boxAmount));
                await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                    user.Id, InventoryCategory.Box, Box.Seaport.GetHashCode(), boxAmount));
                await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                    user.Id, InventoryCategory.Box, Box.Castle.GetHashCode(), boxAmount));
                await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                    user.Id, InventoryCategory.Box, Box.Village.GetHashCode(), boxAmount));

                var embed = new EmbedBuilder()
                    .WithAuthor(IzumiReplyMessage.AdventurePointsRewardAuthor.Parse())
                    .WithDescription(
                        IzumiReplyMessage.AdventurePointsRewardDesc.Parse(
                            displayRowNumber, user.RowNumber) +
                        $"{emotes.GetEmoteOrBlank(Box.Capital.Emote())} {boxAmount} {_local.Localize(Box.Capital.ToString(), boxAmount)}\n" +
                        $"{emotes.GetEmoteOrBlank(Box.Garden.Emote())} {boxAmount} {_local.Localize(Box.Garden.ToString(), boxAmount)}\n" +
                        $"{emotes.GetEmoteOrBlank(Box.Seaport.Emote())} {boxAmount} {_local.Localize(Box.Seaport.ToString(), boxAmount)}\n" +
                        $"{emotes.GetEmoteOrBlank(Box.Castle.Emote())} {boxAmount} {_local.Localize(Box.Castle.ToString(), boxAmount)}\n" +
                        $"{emotes.GetEmoteOrBlank(Box.Village.Emote())} {boxAmount} {_local.Localize(Box.Village.ToString(), boxAmount)}");

                await _mediator.Send(new SendEmbedToUserCommand(
                    await _mediator.Send(new GetDiscordSocketUserQuery(user.Id)), embed));
            }

            await ResetAdventurePoints();
        }

        public async Task ResetAdventurePoints() =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    update users
                    set points = 0,
                        updated_at = now()
                    where points > 0");
    }
}
