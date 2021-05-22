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
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.GameServices.PropertyService.Commands;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.BackgroundJobs.CasinoJob
{
    [InjectableService]
    public class CasinoJob : ICasinoJob
    {
        private readonly IMediator _mediator;

        public CasinoJob(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Open()
        {
            // обновляем состояние казино на 1 - открыто
            await _mediator.Send(new UpdatePropertyCommand(Property.CasinoState, 1));

            var embed = new EmbedBuilder()
                // имя нпс
                .WithAuthor(Npc.Jodi.Name())
                // изображение нпс
                .WithThumbnailUrl(await _mediator.Send(new GetImageUrlQuery(Image.NpcCapitalJodi)))
                // оповещение об открытии казино
                .WithDescription(IzumiEventMessage.CasinoOpen.Parse())
                // изображение казино
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.LocationCapitalCasino)));

            // отправляем сообщение
            var message = await _mediator.Send(new SendEmbedToChannelCommand(DiscordChannel.CapitalEvents, embed));

            // запускаем джобу с удалением сообщения
            BackgroundJob.Schedule<IDiscordJob>(x =>
                    x.DeleteMessage((long) message.Channel.Id, (long) message.Id),
                TimeSpan.FromHours(12));
        }

        public async Task Close()
        {
            // обновляем состояние казино на 0 - закрыто
            await _mediator.Send(new UpdatePropertyCommand(Property.CasinoState, 0));

            var embed = new EmbedBuilder()
                // имя нпс
                .WithAuthor(Npc.Jodi.Name())
                // изображение нпс
                .WithThumbnailUrl(await _mediator.Send(new GetImageUrlQuery(Image.NpcCapitalJodi)))
                // оповещение об закрытии казино
                .WithDescription(IzumiEventMessage.CasinoClosed.Parse())
                // изображение нпс
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.LocationCapitalCasino)));

            // отправляем сообщение
            var message = await _mediator.Send(new SendEmbedToChannelCommand(DiscordChannel.CapitalEvents, embed));

            // запускаем джобу с удалением сообщения
            BackgroundJob.Schedule<IDiscordJob>(x =>
                    x.DeleteMessage((long) message.Channel.Id, (long) message.Id),
                TimeSpan.FromHours(12));
        }
    }
}
