using System;
using System.Threading.Tasks;
using Discord;
using Hangfire;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.RpgServices.FieldService;
using Hinode.Izumi.Services.RpgServices.PropertyService;

namespace Hinode.Izumi.Services.BackgroundJobs.SeasonJob
{
    [InjectableService]
    public class SeasonJob : ISeasonJob
    {
        private readonly IPropertyService _propertyService;
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IFieldService _fieldService;
        private readonly IDiscordGuildService _discordGuildService;

        public SeasonJob(IPropertyService propertyService, IDiscordEmbedService discordEmbedService,
            IEmoteService emoteService, IFieldService fieldService, IDiscordGuildService discordGuildService)
        {
            _propertyService = propertyService;
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _fieldService = fieldService;
            _discordGuildService = discordGuildService;
        }

        public async Task SpringComing() => await NewSeasonComing(Season.Spring);

        public async Task SummerComing() => await NewSeasonComing(Season.Summer);

        public async Task AutumnComing() => await NewSeasonComing(Season.Autumn);

        public async Task WinterComing() => await NewSeasonComing(Season.Winter);

        public async Task UpdateSeason(Season season)
        {
            // сбрасываем все ячейки участков
            await _fieldService.ResetField();
            // обновляем текущий сезон в мире
            await _propertyService.UpdateProperty(Property.CurrentSeason, season.GetHashCode());
        }

        /// <summary>
        /// Оповещает о наступлении нового сезона.
        /// <param name="season">Наступающий сезон.</param>
        /// </summary>
        private async Task NewSeasonComing(Season season)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем текущее время
            var timeNow = DateTimeOffset.Now;
            // получаем каналы дискорда из базы
            var channels = await _discordGuildService.GetChannels();

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

            await _discordEmbedService.SendEmbed(
                // получаем канал дневника в дискорде
                await _discordGuildService.GetSocketTextChannel(
                    channels[DiscordChannel.Diary].Id), embed);
        }
    }
}
