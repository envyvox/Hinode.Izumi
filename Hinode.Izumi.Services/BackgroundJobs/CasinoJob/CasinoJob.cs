using System;
using System.Threading.Tasks;
using Discord;
using Hangfire;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.BackgroundJobs.MessageJob;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.BackgroundJobs.CasinoJob
{
    [InjectableService]
    public class CasinoJob : ICasinoJob
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IDiscordGuildService _discordGuildService;
        private readonly IImageService _imageService;

        public CasinoJob(IDiscordEmbedService discordEmbedService, IDiscordGuildService discordGuildService,
            IImageService imageService)
        {
            _discordEmbedService = discordEmbedService;
            _discordGuildService = discordGuildService;
            _imageService = imageService;
        }

        public async Task Open()
        {
            // получаем роли сервера
            var roles = await _discordGuildService.GetRoles();
            // получаем каналы сервера
            var channels = await _discordGuildService.GetChannels();
            // получаем канал событий столицы
            var channelId = channels[DiscordChannel.CapitalEvents].Id;

            var embed = new EmbedBuilder()
                // имя нпс
                .WithAuthor(Npc.Jodi.Name())
                // изображение нпс
                .WithThumbnailUrl(await _imageService.GetImageUrl(Image.NpcCapitalJodi))
                // оповещение об открытии казино
                .WithDescription(IzumiEventMessage.CasinoOpen.Parse())
                // изображение казино
                .WithImageUrl(await _imageService.GetImageUrl(Image.LocationCapitalCasino));

            // отправляем сообщение
            var message = await (await _discordGuildService.GetSocketTextChannel(channelId))
                .SendMessageAsync(
                    // упоминаем роли события
                    $"<@&{roles[DiscordRole.AllEvents].Id}> <@&{roles[DiscordRole.DailyEvents].Id}>",
                    false, _discordEmbedService.BuildEmbed(embed));

            // запускаем джобу с удалением сообщения
            BackgroundJob.Schedule<IMessageJob>(x =>
                    x.Delete(channelId, (long) message.Id),
                TimeSpan.FromHours(12));
        }

        public async Task Close()
        {
            // получаем каналы сервера
            var channels = await _discordGuildService.GetChannels();
            // получаем канал событий столицы
            var channelId = channels[DiscordChannel.CapitalEvents].Id;

            var embed = new EmbedBuilder()
                // имя нпс
                .WithAuthor(Npc.Jodi.Name())
                // изображение нпс
                .WithThumbnailUrl(await _imageService.GetImageUrl(Image.NpcCapitalJodi))
                // оповещение об закрытии казино
                .WithDescription(IzumiEventMessage.CasinoClosed.Parse())
                // изображение нпс
                .WithImageUrl(await _imageService.GetImageUrl(Image.LocationCapitalCasino));

            // отправляем сообщение
            var message = await (await _discordGuildService.GetSocketTextChannel(channelId))
                .SendMessageAsync("", false,
                    _discordEmbedService.BuildEmbed(embed));

            // запускаем джобу с удалением сообщения
            BackgroundJob.Schedule<IMessageJob>(x =>
                    x.Delete(channelId, (long) message.Id),
                TimeSpan.FromHours(12));
        }
    }
}
