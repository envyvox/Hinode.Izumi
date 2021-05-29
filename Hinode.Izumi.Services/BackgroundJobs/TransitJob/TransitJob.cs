using System;
using System.Threading.Tasks;
using Discord;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.AchievementEnums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.AchievementService.Commands;
using Hinode.Izumi.Services.GameServices.LocationService.Commands;
using Hinode.Izumi.Services.GameServices.LocationService.Queries;
using Hinode.Izumi.Services.GameServices.StatisticService.Commands;
using Hinode.Izumi.Services.GameServices.TutorialService.Commands;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.BackgroundJobs.TransitJob
{
    [InjectableService]
    public class TransitJob : ITransitJob
    {
        private readonly IMediator _mediator;

        public TransitJob(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task CompleteTransit(long userId, Location destination)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());

            // обновляем текущую локацию пользователя
            await _mediator.Send(new UpdateUserLocationCommand(userId, destination));
            // удаляем информацию о перемещении
            await _mediator.Send(new DeleteUserMovementCommand(userId));
            // снимаем с пользователя роль "в пути" в дискорде
            await _mediator.Send(new RemoveDiscordRoleFromUserCommand(userId, DiscordRole.LocationInTransit));
            // добавляем пользователю роль новой локации в дискорде
            await _mediator.Send(new AddDiscordRoleToUserCommand(userId,
                // определяем роль дискорда по локации
                await _mediator.Send(new GetLocationRoleQuery(destination))));

            var embed = new EmbedBuilder()
                // баннер перемещения
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.InTransit)))
                .WithDescription(
                    // оповещаем о завершении перемещения
                    IzumiReplyMessage.TransitCompleteNotify.Parse(
                        destination.Localize()) +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}");

            // если перемещение было не в подлокацию
            if (!destination.SubLocation())
            {
                // добавляем пользователю статистику перемещений
                await _mediator.Send(new AddStatisticToUserCommand(userId, Statistic.Transit));
                // проверяем выполнил ли пользователь достижение
                await _mediator.Send(new CheckAchievementInUserCommand(userId, Achievement.FirstTransit));

                // получаем список каналов дискорда из базы
                var channels = await _mediator.Send(new GetDiscordChannelsQuery());
                DiscordChannel descChannel;
                DiscordChannel whatToDoChannel;
                DiscordChannel eventsChannel;

                // получаем каналы категории города
                // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
                switch (destination)
                {
                    case Location.Capital:
                        descChannel = DiscordChannel.CapitalDesc;
                        whatToDoChannel = DiscordChannel.CapitalWhatToDo;
                        eventsChannel = DiscordChannel.CapitalEvents;
                        break;
                    case Location.Garden:
                        descChannel = DiscordChannel.GardenDesc;
                        whatToDoChannel = DiscordChannel.GardenWhatToDo;
                        eventsChannel = DiscordChannel.GardenEvents;
                        break;
                    case Location.Seaport:
                        descChannel = DiscordChannel.SeaportDesc;
                        whatToDoChannel = DiscordChannel.SeaportWhatToDo;
                        eventsChannel = DiscordChannel.SeaportEvents;
                        break;
                    case Location.Castle:
                        descChannel = DiscordChannel.CastleDesc;
                        whatToDoChannel = DiscordChannel.CastleWhatToDo;
                        eventsChannel = DiscordChannel.CastleEvents;
                        break;
                    case Location.Village:
                        descChannel = DiscordChannel.VillageDesc;
                        whatToDoChannel = DiscordChannel.VillageWhatToDo;
                        eventsChannel = DiscordChannel.VillageEvents;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(destination), destination, null);
                }


                // добавляем embed field с описанием каналов в этой категории
                embed.AddField(IzumiReplyMessage.TransitCompleteInfoChannelsFieldName.Parse(),
                    $"<#{channels[descChannel].Id}>, <#{channels[whatToDoChannel].Id}>, <#{channels[eventsChannel].Id}>");
            }

            await _mediator.Send(new SendEmbedToUserCommand(
                await _mediator.Send(new GetDiscordSocketUserQuery(userId)), embed));

            // проверяем нужно ли двинуть прогресс обучения пользователя
            // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
            switch (destination)
            {
                case Location.Seaport:

                    await _mediator.Send(new CheckUserTutorialStepCommand(
                        userId, TutorialStep.TransitToSeaport));

                    break;
                case Location.Garden:

                    await _mediator.Send(new CheckUserTutorialStepCommand(
                        userId, TutorialStep.TransitToGarden));

                    break;
                case Location.Castle:

                    await _mediator.Send(new CheckUserTutorialStepCommand(
                        userId, TutorialStep.TransitToCastle));

                    break;
                case Location.Village:

                    await _mediator.Send(new CheckUserTutorialStepCommand(
                        userId, TutorialStep.TransitToVillage));

                    break;
                case Location.Capital:

                    await _mediator.Send(new CheckUserTutorialStepCommand(
                        userId, TutorialStep.TransitToCapital));
                    await _mediator.Send(new CheckUserTutorialStepCommand(
                        userId, TutorialStep.TransitToCapitalAfterSeedShop));
                    await _mediator.Send(new CheckUserTutorialStepCommand(
                        userId, TutorialStep.TransitToCapitalAfterMarket));
                    await _mediator.Send(new CheckUserTutorialStepCommand
                        (userId, TutorialStep.TransitToCapitalAfterCasino));

                    break;
                case Location.CapitalShop:

                    await _mediator.Send(new CheckUserTutorialStepCommand(
                        userId, TutorialStep.TransitToCapitalShop));

                    break;
                case Location.CapitalMarket:

                    await _mediator.Send(new CheckUserTutorialStepCommand(
                        userId, TutorialStep.TransitToCapitalMarket));

                    break;
                case Location.CapitalCasino:

                    await _mediator.Send(new CheckUserTutorialStepCommand(
                        userId, TutorialStep.TransitToCapitalCasino));

                    break;
            }
        }
    }
}
