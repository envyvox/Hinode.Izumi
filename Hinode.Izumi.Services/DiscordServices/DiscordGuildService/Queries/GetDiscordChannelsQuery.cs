using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Discord;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Records;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;


namespace Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries
{
    public record GetDiscordChannelsQuery : IRequest<Dictionary<DiscordChannel, DiscordChannelRecord>>;

    public class GetDiscordChannelsHandler
        : IRequestHandler<GetDiscordChannelsQuery, Dictionary<DiscordChannel, DiscordChannelRecord>>
    {
        private readonly IMemoryCache _cache;
        private readonly IConnectionManager _con;
        private readonly IMediator _mediator;

        private const string DiscordChannelsKey = "discord_channels";

        public GetDiscordChannelsHandler(IMemoryCache cache, IConnectionManager con, IMediator mediator)
        {
            _cache = cache;
            _con = con;
            _mediator = mediator;
        }

        public async Task<Dictionary<DiscordChannel, DiscordChannelRecord>> Handle(GetDiscordChannelsQuery request,
            CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(DiscordChannelsKey, out Dictionary<DiscordChannel, DiscordChannelRecord> channels))
                return channels;

            channels = (await _con.GetConnection()
                    .QueryAsync<DiscordChannelRecord>(@"
                        select * from discord_channels"))
                .ToDictionary(x => x.Channel);

            var channelTypes = Enum.GetValues(typeof(DiscordChannel))
                .Cast<DiscordChannel>()
                .ToArray();

            // проверяем есть ли все каналы в библиотеке
            if (channels.Count < channelTypes.Length)
            {
                // если количество не совпадает - нужно создать и добавить недостающие каналы
                foreach (var channel in channelTypes)
                {
                    // если такой канал уже есть в библиотеке - пропускаем
                    if (channels.ContainsKey(channel)) continue;

                    // получаем сервер дискорда
                    var guild = await _mediator.Send(new GetDiscordSocketGuildQuery(), cancellationToken);
                    // ищем канал на сервере
                    var chan = guild.Channels.FirstOrDefault(x => x.Name == channel.Name());

                    // если такого канала нет - его нужно создать
                    if (chan is null)
                    {
                        // создание канала отличается в зависимости от его категории
                        switch (channel.Category())
                        {
                            case DiscordChannelCategory.TextChannel:

                                // создаем текстовый канал
                                var textChannel = await guild.CreateTextChannelAsync(channel.Name(), x =>
                                {
                                    // и указываем его родительский канал, если он имеется
                                    x.CategoryId = channels.ContainsKey(channel.Parent())
                                        ? (ulong) channels[channel.Parent()].Id
                                        : Optional<ulong?>.Unspecified;
                                });

                                // добавляем канал в библиотеку
                                channels.Add(channel, new DiscordChannelRecord((long) textChannel.Id, channel));
                                // добавляем канал в базу
                                await AddDiscordChannel((long) textChannel.Id, channel);

                                break;
                            case DiscordChannelCategory.VoiceChannel:

                                // создаем голосовой канал
                                var voiceChannel = await guild.CreateVoiceChannelAsync(channel.Name(), x =>
                                {
                                    // и указываем его родительский канал, если он имеется
                                    x.CategoryId = channels.ContainsKey(channel.Parent())
                                        ? (ulong) channels[channel.Parent()].Id
                                        : Optional<ulong?>.Unspecified;
                                });

                                // добавляем канал в библиотеку
                                channels.Add(channel, new DiscordChannelRecord((long) voiceChannel.Id, channel));
                                // добавляем канал в базу
                                await AddDiscordChannel((long) voiceChannel.Id, channel);

                                break;
                            case DiscordChannelCategory.CategoryChannel:

                                // создаем родительский канал
                                var categoryChannel = await guild.CreateCategoryChannelAsync(channel.Name());

                                // добавляем канал в библиотеку
                                channels.Add(channel, new DiscordChannelRecord((long) categoryChannel.Id, channel));
                                // добавляем канал в базу
                                await AddDiscordChannel((long) categoryChannel.Id, channel);

                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    // если канал найден - его нужно просто добавить в библиотеку
                    else
                    {
                        // добавляем в библиотеку
                        channels.Add(channel, new DiscordChannelRecord((long) chan.Id, channel));
                        // добавляем в базу
                        await AddDiscordChannel((long) chan.Id, channel);
                    }
                }
            }

            _cache.Set(DiscordChannelsKey, channels, CacheExtensions.DefaultCacheOptions);

            return channels;
        }

        private async Task AddDiscordChannel(long id, DiscordChannel channel) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into discord_channels(id, channel)
                    values (@id, @channel)
                    on conflict (channel) do update
                        set id = @id,
                            updated_at = now()",
                    new {id, channel});
    }
}
