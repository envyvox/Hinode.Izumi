using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.DiscordServices.DiscordClientService;
using Hinode.Izumi.Services.EmoteService.Models;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.EmoteService.Impl
{
    [InjectableService]
    public class EmoteService : IEmoteService
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;
        private readonly IDiscordClientService _discordClientService;

        public EmoteService(IConnectionManager con, IMemoryCache cache, IDiscordClientService discordClientService)
        {
            _con = con;
            _cache = cache;
            _discordClientService = discordClientService;
        }

        public async Task<Dictionary<string, EmoteModel>> GetEmotes()
        {
            // проверяем иконки в кэше
            if (_cache.TryGetValue(CacheExtensions.EmotesKey, out Dictionary<string, EmoteModel> emotes)) return emotes;

            // получаем иконки из базы
            emotes = (await _con.GetConnection()
                    .QueryAsync<EmoteModel>(@"
                        select * from emotes"))
                .ToDictionary(x => x.Name);

            // добавляем иконки в кэш
            _cache.Set(CacheExtensions.EmotesKey, emotes, CacheExtensions.DefaultCacheOptions);

            // возвращаем иконки
            return emotes;
        }

        public async Task UploadEmotes()
        {
            // сначала нужно удалить все иконки чтобы отсеять удаленные
            await DeleteEmotes();

            // получаем клиент
            var socketClient = await _discordClientService.GetSocketClient();
            // создаем списки в которые будет добавлять информацию о иконках
            var emoteId = new List<long>();
            var emoteName = new List<string>();
            var emoteCode = new List<string>();

            // для каждого сервера нужно получить его иконки
            foreach (var socketGuild in socketClient.Guilds)
            {
                foreach (var socketGuildEmote in socketGuild.Emotes)
                {
                    // добавляем информацию о иконке в списки
                    emoteId.Add((long) socketGuildEmote.Id);
                    emoteName.Add(socketGuildEmote.Name);
                    emoteCode.Add(socketGuildEmote.ToString());
                }
            }

            // добавляем иконки в базу
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into emotes(id, name, code)
                    values (
                            unnest(array[@emoteId]),
                            unnest(array[@emoteName]),
                            unnest(array[@emoteCode])
                            )
                    on conflict (id, name) do nothing",
                    new {emoteId, emoteName, emoteCode});
        }

        private async Task DeleteEmotes()
        {
            // удаляем иконки из базы
            await _con
                .GetConnection()
                .ExecuteAsync(@"
                    delete from emotes");

            // удаляем иконки из кэша
            _cache.Remove(CacheExtensions.EmotesKey);
        }
    }
}
