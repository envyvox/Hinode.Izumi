using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hangfire;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.LocationService.Commands;
using Hinode.Izumi.Services.GameServices.LocationService.Queries;
using Hinode.Izumi.Services.HangfireJobService.Commands;
using Hinode.Izumi.Services.HangfireJobService.Queries;
using Hinode.Izumi.Services.WebServices.CommandWebService.Attributes;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands.ExploreCommands
{
    [CommandCategory(
        CommandCategory.Explore,
        CommandCategory.Crafting,
        CommandCategory.Cooking,
        CommandCategory.Field,
        CommandCategory.Transit)]
    [IzumiRequireRegistry]
    public class InterruptAction : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;

        private readonly Location[] _interruptedLocations =
        {
            Location.Fishing, Location.ExploreCastle, Location.ExploreGarden, Location.FieldWatering,
            Location.MakingCrafting, Location.MakingAlcohol, Location.MakingFood, Location.MakingDrink
        };

        public InterruptAction(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Command("прерваться")]
        [Summary("Прерывает текущее действие.")]
        public async Task InterruptExploreTask()
        {
            var emotes = await _mediator.Send(new GetEmotesQuery());
            var userLocation = await _mediator.Send(new GetUserLocationQuery((long) Context.User.Id));

            if (_interruptedLocations.Contains(userLocation))
            {
                var userMovement = await _mediator.Send(new GetUserMovementQuery((long) Context.User.Id));
                var hangfireAction = userLocation switch
                {
                    Location.ExploreGarden => HangfireAction.Explore,
                    Location.ExploreCastle => HangfireAction.Explore,
                    Location.Fishing => HangfireAction.Explore,
                    Location.FieldWatering => HangfireAction.FieldWatering,
                    Location.MakingCrafting => HangfireAction.Making,
                    Location.MakingAlcohol => HangfireAction.Making,
                    Location.MakingFood => HangfireAction.Making,
                    Location.MakingDrink => HangfireAction.Making,
                    _ => throw new ArgumentOutOfRangeException()
                };
                var userHangfireJobId = await _mediator.Send(new GetUserHangfireJobIdQuery(
                    (long) Context.User.Id, hangfireAction));

                await _mediator.Send(new UpdateUserLocationCommand((long) Context.User.Id, userMovement.Destination));
                await _mediator.Send(new DeleteUserMovementCommand((long) Context.User.Id));
                await _mediator.Send(new DeleteUserHangfireJobCommand((long) Context.User.Id, hangfireAction));
                BackgroundJob.Delete(userHangfireJobId);

                var embed = new EmbedBuilder()
                    .WithDescription(IzumiReplyMessage.InterruptActionSuccess.Parse());

                await _mediator.Send(new SendEmbedToUserCommand(Context.User, embed));
            }
            else
            {
                await Task.FromException(new Exception(IzumiReplyMessage.InterruptActionAcceptedList.Parse(
                    _interruptedLocations.Aggregate(string.Empty,
                        (current, location) =>
                            current + $"{emotes.GetEmoteOrBlank("List")} {location.Localize()}\n"))));
            }
        }
    }
}
