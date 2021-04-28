using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.EffectEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.RpgServices.CardService.Models;
using Hinode.Izumi.Services.RpgServices.EffectService;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.RpgServices.CardService.Impl
{
    [InjectableService]
    public class CardService : ICardService
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;
        private readonly IEffectService _effectService;

        public CardService(IConnectionManager con, IMemoryCache cache, IEffectService effectService)
        {
            _con = con;
            _cache = cache;
            _effectService = effectService;
        }

        public async Task<CardModel> GetCard(long id)
        {
            // проверяем карточку в кэше
            if (_cache.TryGetValue(string.Format(CacheExtensions.CardKey, id), out CardModel card)) return card;

            // получаем карточку из базы
            card = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<CardModel>(@"
                    select * from cards
                    where id = @id",
                    new {id});

            // если в базе есть такая карточка
            if (card != null)
            {
                // добавляем ее в кэш
                _cache.Set(string.Format(CacheExtensions.CardKey, id), card, CacheExtensions.DefaultCacheOptions);
                // и возвращаем
                return card;
            }

            // если в базе нет такой карточки - выводим ошибку
            await Task.FromException(new Exception(IzumiNullableMessage.CardById.Parse()));
            return new CardModel();
        }

        public async Task<CardModel> GetUserCard(long userId, long cardId)
        {
            // получаем карточку пользователя
            var res = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<CardModel>(@"
                    select c.* from user_cards as uc
                        inner join cards c
                            on c.id = uc.card_id
                    where uc.user_id = @userId
                      and uc.card_id = @cardId",
                    new {userId, cardId});

            // если у пользователя нет такой карточки - выводим ошибку
            if (res == null)
                await Task.FromException(new Exception(IzumiNullableMessage.UserCard.Parse()));

            // возвращаем карточку пользователя
            return res;
        }

        public async Task<CardModel[]> GetUserCard(long userId) =>
            (await _con.GetConnection()
                .QueryAsync<CardModel>(@"
                    select c.* from user_cards as uc
                        inner join cards c
                            on c.id = uc.card_id
                    where uc.user_id = @userId",
                    new {userId}))
            .ToArray();

        public async Task<CardModel[]> GetUserDeck(long userId) =>
            (await _con.GetConnection()
                .QueryAsync<CardModel>(@"
                    select c.* from user_decks as ud
                        inner join cards c
                            on c.id = ud.card_id
                    where ud.user_id = @userId",
                    new {userId}))
            .ToArray();

        public async Task<bool> CheckCardInUserDeck(long userId, long cardId) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<bool>(@"
                    select 1 from user_decks
                    where user_id = @userId
                      and card_id = @card_id",
                    new {userId, cardId});

        public async Task<long> GetUserDeckLength(long userId) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<long>(@"
                    select count(*) from user_decks
                    where user_id = @userId",
                    new {userId});

        public async Task<long> GetAllCardLength() =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<long>(@"
                    select count(*) from cards");

        public async Task AddCardToUser(long userId, long cardId) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_cards(user_id, card_id)
                    values (@userId, @cardId)
                    on conflict (user_id, card_id) do nothing",
                    new {userId, cardId});

        public async Task AddCardToDeck(long userId, long cardId)
        {
            // добавляем карточку в колоду пользователя
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_decks(user_id, card_id)
                    values (@userId, @cardId)
                    on conflict (user_id, card_id) do nothing",
                    new {userId, cardId});

            // получаем карточку из базы
            var card = await GetCard(cardId);
            // добавляем пользователю эффект карточки
            await _effectService.AddEffectToUser(userId, card.Effect.Category(), card.Effect);
        }

        public async Task RemoveCardFromDeck(long userId, long cardId)
        {
            // убираем карточку из колоды пользователя
            await _con
                .GetConnection()
                .ExecuteAsync(@"
                    delete from user_decks
                    where user_id = @userId
                      and card_id = @cardId",
                    new {userId, cardId});

            // получаем карточку из базы
            var card = await GetCard(cardId);
            // снимаем с пользователя эффект карточки
            await _effectService.RemoveEffectFromUser(userId, card.Effect);
        }
    }
}
