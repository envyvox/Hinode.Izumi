using System;
using System.Threading.Tasks;
using Discord;
using Hangfire;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.GameServices.FieldService.Commands;
using Hinode.Izumi.Services.GameServices.PropertyService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.BackgroundJobs.SeasonJob
{
    [InjectableService]
    public class SeasonJob : ISeasonJob
    {
        private readonly IMediator _mediator;

        public SeasonJob(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task SpringComing() => await NewSeasonComing(Season.Spring);

        public async Task SummerComing() => await NewSeasonComing(Season.Summer);

        public async Task AutumnComing() => await NewSeasonComing(Season.Autumn);

        public async Task WinterComing() => await NewSeasonComing(Season.Winter);

        public async Task UpdateSeason(Season season)
        {
            // сбрасываем все ячейки участков
            await _mediator.Send(new ResetAllFieldsCommand());
            // обновляем текущий сезон в мире
            await _mediator.Send(new UpdatePropertyCommand(Property.CurrentSeason, season.GetHashCode()));
        }

        /// <summary>
        /// Оповещает о наступлении нового сезона.
        /// <param name="season">Наступающий сезон.</param>
        /// </summary>
        private async Task NewSeasonComing(Season season)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем текущее время
            var timeNow = DateTimeOffset.Now;

            // добавляем джобу со сменой сезона через неделю
            BackgroundJob.Schedule<ISeasonJob>(
                x => x.UpdateSeason(season),
                timeNow.AddDays(7));

            var embed = new EmbedBuilder()
                .WithAuthor(IzumiEventMessage.DiaryAuthorField.Parse())
                .WithDescription(
                    // определяем текст в зависимости от сезона
                    season switch
                    {
                        Season.Spring => IzumiEventMessage.SpringComing.Parse(),
                        Season.Summer => IzumiEventMessage.SummerComing.Parse(),
                        Season.Autumn => IzumiEventMessage.AutumnComing.Parse(),
                        Season.Winter => IzumiEventMessage.WinterComing.Parse(),
                        Season.Any => throw new ArgumentOutOfRangeException(nameof(season), season, null),
                        _ => throw new ArgumentOutOfRangeException(nameof(season), season, null)
                    });

            await _mediator.Send(new SendEmbedToChannelCommand(DiscordChannel.Diary, embed));
        }
    }
}
