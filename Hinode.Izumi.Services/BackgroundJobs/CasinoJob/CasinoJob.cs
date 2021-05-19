using System;
using System.Threading.Tasks;
using Discord;
using Hangfire;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.BackgroundJobs.DiscordJob;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.PropertyService;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.BackgroundJobs.CasinoJob
{
    [InjectableService]
    public class CasinoJob : ICasinoJob
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IImageService _imageService;
        private readonly IPropertyService _propertyService;

        public CasinoJob(IDiscordEmbedService discordEmbedService, IImageService imageService,
            IPropertyService propertyService)
        {
            _discordEmbedService = discordEmbedService;
            _imageService = imageService;
            _propertyService = propertyService;
        }

        public async Task Open()
        {
            // обновляем состояние казино на 1 - открыто
            await _propertyService.UpdateProperty(Property.CasinoState, 1);

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
            var message = await _discordEmbedService.SendEmbed(DiscordChannel.CapitalEvents, embed);

            // запускаем джобу с удалением сообщения
            BackgroundJob.Schedule<IDiscordJob>(x =>
                    x.DeleteMessage((long) message.Channel.Id, (long) message.Id),
                TimeSpan.FromHours(12));
        }

        public async Task Close()
        {
            // обновляем состояние казино на 0 - закрыто
            await _propertyService.UpdateProperty(Property.CasinoState, 0);

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
            var message = await _discordEmbedService.SendEmbed(DiscordChannel.CapitalEvents, embed);

            // запускаем джобу с удалением сообщения
            BackgroundJob.Schedule<IDiscordJob>(x =>
                    x.DeleteMessage((long) message.Channel.Id, (long) message.Id),
                TimeSpan.FromHours(12));
        }
    }
}
