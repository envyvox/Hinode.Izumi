using System;
using System.Threading.Tasks;
using Discord;
using Hangfire;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using Hinode.Izumi.Services.GameServices.GatheringService.Commands;
using Hinode.Izumi.Services.GameServices.PropertyService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.BackgroundJobs.EventBackgroundJobs.EventJuneJob
{
    [InjectableService]
    public class EventJuneJob : IEventJuneJob
    {
        private readonly IMediator _mediator;

        public EventJuneJob(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Start()
        {
            // получаем роли сервера
            var roles = await _mediator.Send(new GetDiscordRolesQuery());
            // обновляем текущее событие в базе
            await _mediator.Send(new UpdatePropertyCommand(Property.CurrentEvent, (long) Event.June));

            var embed = new EmbedBuilder()
                .WithDescription("Июньское событие начинается.");

            await _mediator.Send(new SendEmbedToChannelCommand(DiscordChannel.Diary, embed,
                // упоминаем роли события
                $"<@&{roles[DiscordRole.AllEvents].Id}> <@&{roles[DiscordRole.YearlyEvents].Id}>"));
        }

        public async Task End()
        {
            // обновляем текущее событие в базе
            await _mediator.Send(new UpdatePropertyCommand(Property.CurrentEvent, (long) Event.None));

            var embed = new EmbedBuilder()
                .WithDescription("Июньское событие закончилось.");

            await _mediator.Send(new SendEmbedToChannelCommand(DiscordChannel.Diary, embed));

            // добавляем джобу удаление собирательских предметов события
            BackgroundJob.Schedule<IEventJuneJob>(
                x => x.RemoveEventGatheringFromUsers(),
                TimeSpan.FromDays(30));
        }

        public async Task RemoveEventGatheringFromUsers() =>
            await _mediator.Send(new RemoveEventGatheringFromAllUsersCommand(Event.June));
    }
}
