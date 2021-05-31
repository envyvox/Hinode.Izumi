using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hangfire;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.BackgroundJobs.ExploreJob;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using Hinode.Izumi.Services.GameServices.GatheringService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.LocationService.Commands;
using Hinode.Izumi.Services.GameServices.MasteryService.Queries;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using Hinode.Izumi.Services.GameServices.UserService.Commands;
using Hinode.Izumi.Services.GameServices.UserService.Queries;
using Hinode.Izumi.Services.HangfireJobService.Commands;
using Hinode.Izumi.Services.ImageService.Queries;
using Humanizer;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.ExploreCommands.ExploreCastleCommand
{
    [InjectableService]
    public class ExploreCastleCommand : IExploreCastleCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public ExploreCastleCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем текущее время
            var timeNow = DateTimeOffset.Now;
            // получаем пользователя из базы
            var user = await _mediator.Send(new GetUserByIdQuery((long) context.User.Id));
            // получаем мастерство сбора пользователя
            var userMastery = await _mediator.Send(new GetUserMasteryQuery((long) context.User.Id, Mastery.Gathering));
            // округляем мастерство
            var masteryAmount = (long) Math.Floor(userMastery.Amount);
            // определяем длительность исследования
            var exploreTime = await _mediator.Send(new GetActionTimeQuery(
                await _mediator.Send(new GetGatheringTimeQuery(masteryAmount)), user.Energy));
            // получаем собирательские ресурсы этой локации
            var gatherings = await _mediator.Send(new GetGatheringsInLocationQuery(Location.Castle));

            // обновляем текущую локацию пользователя
            await _mediator.Send(new UpdateUserLocationCommand((long) context.User.Id, Location.ExploreCastle));
            // добавляем информацию о перемещении
            await _mediator.Send(new CreateUserMovementCommand((long) context.User.Id,
                Location.ExploreCastle, Location.Castle, timeNow.AddMinutes(exploreTime)));
            // отнимаем энергию у пользователя
            await _mediator.Send(new RemoveEnergyFromUserCommand((long) context.User.Id,
                await _mediator.Send(new GetPropertyValueQuery(Property.EnergyCostExplore))));

            // запускаем джобу для окончания исследования
            var jobId = BackgroundJob.Schedule<IExploreJob>(x =>
                    x.CompleteExploreCastle((long) context.User.Id, masteryAmount),
                TimeSpan.FromMinutes(exploreTime));
            await _mediator.Send(new CreateUserHangfireJobCommand(
                (long) context.User.Id, HangfireAction.Explore, jobId));


            // выводим собирательские ресурсы этой локации
            var resourcesString = gatherings.Aggregate(string.Empty, (current, gathering) =>
                current + $"{emotes.GetEmoteOrBlank(gathering.Name)} {_local.Localize(gathering.Name)}, ");
            var embed = new EmbedBuilder()
                .WithAuthor(Location.ExploreCastle.Localize())
                // баннер исследования сада
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.ExploreCastle)))
                // подверждаем что исследование начато
                .WithDescription(
                    IzumiReplyMessage.ExploreCastleBegin.Parse(Location.Garden.Localize()) +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // ожидаемые награды
                .AddField(IzumiReplyMessage.ExploreRewardFieldName.Parse(),
                    resourcesString.Remove(resourcesString.Length - 2))
                // длительность
                .AddField(IzumiReplyMessage.TimeFieldName.Parse(),
                    exploreTime.Minutes().Humanize(2, new CultureInfo("ru-RU")));

            await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
            await Task.CompletedTask;
        }
    }
}
