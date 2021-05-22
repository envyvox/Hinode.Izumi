using System;
using System.Globalization;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hangfire;
using Hinode.Izumi.Commands.Attributes;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.BackgroundJobs.ExploreJob;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.BuildingService.Queries;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using Hinode.Izumi.Services.GameServices.LocationService.Commands;
using Hinode.Izumi.Services.GameServices.MasteryService.Queries;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using Hinode.Izumi.Services.GameServices.UserService.Commands;
using Hinode.Izumi.Services.GameServices.UserService.Queries;
using Hinode.Izumi.Services.ImageService.Queries;
using Humanizer;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.ExploreCommands
{
    [CommandCategory(CommandCategory.Explore)]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    [IzumiRequireLocation(Location.Seaport), IzumiRequireNoDebuff(BossDebuff.SeaportStop)]
    public class FishingCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;

        public FishingCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Command("рыбачить"), Alias("fishing")]
        [Summary("Отправиться на рыбалку")]
        public async Task FishingTask()
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем текущее время
            var timeNow = DateTimeOffset.Now;
            // получаем пользователя из базы
            var user = await _mediator.Send(new GetUserByIdQuery((long) Context.User.Id));
            // получаем мастерство рыбалки пользователя
            var userMastery = await _mediator.Send(new GetUserMasteryQuery((long) Context.User.Id, Mastery.Fishing));

            // проверяем есть ли у пользователя рыбацкая лодка
            var hasFishingBoat = await _mediator.Send(new CheckBuildingInUserQuery(
                (long) Context.User.Id, Building.FishingBoat));
            // определяем длительность рыбалки
            var fishingTime = await _mediator.Send(new GetFishingTimeQuery(user.Energy, hasFishingBoat));

            // обновляем текущую локацию пользователя
            await _mediator.Send(new UpdateUserLocationCommand((long) Context.User.Id, Location.Fishing));
            // добавляем информацию о перемещении
            await _mediator.Send(new CreateUserMovementCommand((long) Context.User.Id,
                Location.Fishing, Location.Seaport, timeNow.AddMinutes(fishingTime)));
            // отнимаем энергию у пользователя
            await _mediator.Send(new RemoveEnergyFromUserCommand((long) Context.User.Id,
                await _mediator.Send(new GetPropertyValueQuery(Property.EnergyCostExplore))));

            // запускаем джобу для окончания рыбалки
            BackgroundJob.Schedule<IExploreJob>(x =>
                    x.CompleteFishing((long) Context.User.Id, (long) Math.Floor(userMastery.Amount)),
                TimeSpan.FromMinutes(fishingTime));

            var embed = new EmbedBuilder()
                .WithAuthor(Location.Fishing.Localize())
                // баннер рыбалки
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.Fishing)))
                // подтверждаем успешное начало рыбалки
                .WithDescription(
                    IzumiReplyMessage.FishingBegin.Parse(Location.Seaport.Localize()) +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // ожидаемая награда
                .AddField(IzumiReplyMessage.ExploreRewardFieldName.Parse(),
                    IzumiReplyMessage.ExploreRewardFishingFieldDesc.Parse(
                        emotes.GetEmoteOrBlank("SlimejackBW")), true)
                // длительность
                .AddField(IzumiReplyMessage.TimeFieldName.Parse(),
                    fishingTime.Minutes().Humanize(2, new CultureInfo("ru-RU")), true);

            await _mediator.Send(new SendEmbedToUserCommand(Context.User, embed));
            await Task.CompletedTask;
        }
    }
}
