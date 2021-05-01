using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.AlcoholService;
using Hinode.Izumi.Services.RpgServices.CraftingService;
using Hinode.Izumi.Services.RpgServices.CropService;
using Hinode.Izumi.Services.RpgServices.DrinkService;
using Hinode.Izumi.Services.RpgServices.FishService;
using Hinode.Izumi.Services.RpgServices.FoodService;
using Hinode.Izumi.Services.RpgServices.GatheringService;
using Hinode.Izumi.Services.RpgServices.InventoryService.Models;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.ProductService;
using Hinode.Izumi.Services.RpgServices.SeedService;
using Hinode.Izumi.Services.RpgServices.StatisticService;

namespace Hinode.Izumi.Services.RpgServices.InventoryService.Impl
{
    [InjectableService]
    public class InventoryService : IInventoryService
    {
        private readonly IConnectionManager _con;
        private readonly IEmoteService _emoteService;
        private readonly IFishService _fishService;
        private readonly IFoodService _foodService;
        private readonly IStatisticService _statisticService;
        private readonly ISeedService _seedService;
        private readonly ICropService _cropService;
        private readonly ILocalizationService _local;
        private readonly IGatheringService _gatheringService;
        private readonly IProductService _productService;
        private readonly IAlcoholService _alcoholService;
        private readonly IDrinkService _drinkService;
        private readonly ICraftingService _craftingService;

        public InventoryService(IConnectionManager con, IEmoteService emoteService, IFishService fishService,
            IFoodService foodService, IStatisticService statisticService, ISeedService seedService,
            ICropService cropService, ILocalizationService local, IGatheringService gatheringService,
            IProductService productService, IAlcoholService alcoholService, IDrinkService drinkService,
            ICraftingService craftingService)
        {
            _con = con;
            _emoteService = emoteService;
            _fishService = fishService;
            _foodService = foodService;
            _statisticService = statisticService;
            _seedService = seedService;
            _cropService = cropService;
            _local = local;
            _gatheringService = gatheringService;
            _productService = productService;
            _alcoholService = alcoholService;
            _drinkService = drinkService;
            _craftingService = craftingService;
        }

        public async Task<UserGatheringModel> GetUserGathering(long userId, long gatheringId)
        {
            // получаем собирательский предмет пользователя
            var res = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserGatheringModel>(@"
                    select ug.*, g.name from user_gatherings as ug
                        inner join gatherings g
                            on g.id = ug.gathering_id
                    where ug.user_id = @userId
                      and ug.gathering_id = @gatheringId",
                    new {userId, gatheringId});

            // если собирательский предмет у пользователя есть - возвращаем его
            if (res != null) return res;

            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем собирательский предмет
            var gathering = await _gatheringService.GetGathering(gatheringId);
            // выводим ошибку
            await Task.FromException(new Exception(IzumiNullableMessage.UserInventory.Parse(
                emotes.GetEmoteOrBlank(gathering.Name), _local.Localize(gathering.Name, 2))));

            return new UserGatheringModel();
        }

        public async Task<UserGatheringModel[]> GetUserGathering(long userId) =>
            (await _con.GetConnection()
                .QueryAsync<UserGatheringModel>(@"
                    select ug.*, g.name from user_gatherings as ug
                        inner join gatherings g
                            on g.id = ug.gathering_id
                    where ug.user_id = @userId
                    order by g.id",
                    new {userId}))
            .ToArray();

        public async Task<UserProductModel> GetUserProduct(long userId, long productId)
        {
            // получаем продукт пользователя
            var res = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserProductModel>(@"
                    select up.*, p.name from user_products as up
                        inner join products p
                            on p.id = up.product_id
                    where up.user_id = @userId
                      and up.product_id = @productId",
                    new {userId, productId});

            // если продукт у пользователя есть - возвращаем его
            if (res != null) return res;

            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем продукт
            var product = await _productService.GetProduct(productId);
            // выводим ошибку
            await Task.FromException(new Exception(IzumiNullableMessage.UserInventory.Parse(
                emotes.GetEmoteOrBlank(product.Name), _local.Localize(product.Name, 2))));

            return new UserProductModel();
        }

        public async Task<UserProductModel[]> GetUserProduct(long userId) =>
            (await _con.GetConnection()
                .QueryAsync<UserProductModel>(@"
                    select up.*, p.name from user_products as up
                        inner join products p
                            on p.id = up.product_id
                    where up.user_id = @userId
                    order by p.id",
                    new {userId}))
            .ToArray();

        public async Task<UserCraftingModel> GetUserCrafting(long userId, long craftingId)
        {
            // получаем изготавливаемый предмет пользователя
            var res = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserCraftingModel>(@"
                    select uc.*, c.name from user_craftings as uc
                        inner join craftings c
                            on c.id = uc.crafting_id
                    where uc.user_id = @userId
                      and uc.crafting_id = @craftingId",
                    new {userId, craftingId});

            // если изготавливаемый предмет у пользователя есть - возвращаем его
            if (res != null) return res;

            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем изготавливаемый предмет
            var crafting = await _craftingService.GetCrafting(craftingId);
            // выводим ошибку
            await Task.FromException(new Exception(IzumiNullableMessage.UserInventory.Parse(
                emotes.GetEmoteOrBlank(crafting.Name), _local.Localize(crafting.Name, 2))));

            return new UserCraftingModel();
        }

        public async Task<UserCraftingModel[]> GetUserCrafting(long userId) =>
            (await _con.GetConnection()
                .QueryAsync<UserCraftingModel>(@"
                    select uc.*, c.name from user_craftings as uc
                        inner join craftings c
                            on c.id = uc.crafting_id
                    where uc.user_id = @userId
                    order by c.id",
                    new {userId}))
            .ToArray();

        public async Task<UserAlcoholModel> GetUserAlcohol(long userId, long alcoholId)
        {
            // получаем алкоголь пользователя
            var res = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserAlcoholModel>(@"
                    select ua.*, a.name from user_alcohols as ua
                        inner join alcohols a
                            on a.id = ua.alcohol_id
                    where ua.user_id = @userId
                      and ua.alcohol_id = @alcoholId",
                    new {userId, alcoholId});

            // если алкоголь у пользователя есть - возвращаем его
            if (res != null) return res;

            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем алкоголь
            var alcohol = await _alcoholService.GetAlcohol(alcoholId);
            // выводим ошибку
            await Task.FromException(new Exception(IzumiNullableMessage.UserInventory.Parse(
                emotes.GetEmoteOrBlank(alcohol.Name), _local.Localize(alcohol.Name, 2))));

            return new UserAlcoholModel();
        }

        public async Task<UserAlcoholModel[]> GetUserAlcohol(long userId) =>
            (await _con.GetConnection()
                .QueryAsync<UserAlcoholModel>(@"
                    select ua.*, a.name from user_alcohols as ua
                        inner join alcohols a
                            on a.id = ua.alcohol_id
                    where ua.user_id = @userId
                    order by a.id",
                    new {userId}))
            .ToArray();

        public async Task<UserDrinkModel> GetUserDrink(long userId, long drinkId)
        {
            // получаем напиток пользователя
            var res = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserDrinkModel>(@"
                    select ud.*, d.name from user_drinks as ud
                        inner join drinks d
                            on d.id = ud.drink_id
                    where ud.user_id = @userId
                      and ud.drink_id = @drinkId",
                    new {userId, drinkId});

            // если напиток у пользователя есть - возвращаем его
            if (res != null) return res;

            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем напиток
            var drink = await _drinkService.GetDrink(drinkId);
            // выводим ошибку
            await Task.FromException(new Exception(IzumiNullableMessage.UserInventory.Parse(
                emotes.GetEmoteOrBlank(drink.Name), _local.Localize(drink.Name, 2))));

            return new UserDrinkModel();
        }

        public async Task<UserDrinkModel[]> GetUserDrink(long userId) =>
            (await _con.GetConnection()
                .QueryAsync<UserDrinkModel>(@"
                    select ud.*, d.name from user_drinks as ud
                        inner join drinks d
                            on d.id = ud.drink_id
                    where ud.user_id = @userId
                    order by d.id",
                    new {userId}))
            .ToArray();

        public async Task<UserSeedModel> GetUserSeed(long userId, long seedId)
        {
            // получаем семена пользователя
            var res = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserSeedModel>(@"
                    select us.*, s.name, s.season from user_seeds as us
                        inner join seeds s
                            on s.id = us.seed_id
                    where us.user_id = @userId
                      and us.seed_id = @seedId",
                    new {userId, seedId});

            // если семена у пользователя есть - возвращаем их
            if (res != null) return res;

            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем семена
            var seed = await _seedService.GetSeed(seedId);
            // выводим ошибку
            await Task.FromException(new Exception(IzumiNullableMessage.UserInventory.Parse(
                emotes.GetEmoteOrBlank(seed.Name), _local.Localize(seed.Name, 2))));

            return new UserSeedModel();
        }

        public async Task<UserSeedModel[]> GetUserSeed(long userId) =>
            (await _con.GetConnection()
                .QueryAsync<UserSeedModel>(@"
                    select us.*, s.name, s.season from user_seeds as us
                        inner join seeds s
                            on s.id = us.seed_id
                    where us.user_id = @userId
                    order by s.id",
                    new {userId}))
            .ToArray();

        public async Task<UserCropModel> GetUserCrop(long userId, long cropId)
        {
            // получаем урожай пользователя
            var res = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserCropModel>(@"
                    select uc.*, c.name, s.season from user_crops as uc
                        inner join crops c
                            on c.id = uc.crop_id
                        inner join seeds s
                            on s.id = c.seed_id
                    where uc.user_id = @userId
                      and uc.crop_id = @cropId",
                    new {userId, cropId});

            // если урожай у пользователя есть - возвращаем его
            if (res != null) return res;

            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем урожай
            var crop = await _cropService.GetCrop(cropId);
            // выводим ошибку
            await Task.FromException(new Exception(IzumiNullableMessage.UserInventory.Parse(
                emotes.GetEmoteOrBlank(crop.Name), _local.Localize(crop.Name, 2))));

            return new UserCropModel();
        }

        public async Task<UserCropModel[]> GetUserCrop(long userId) =>
            (await _con.GetConnection()
                .QueryAsync<UserCropModel>(@"
                    select uc.*, c.name, s.season from user_crops as uc
                        inner join crops c
                            on c.id = uc.crop_id
                        inner join seeds s
                            on s.id = c.seed_id
                    where uc.user_id = @userId
                    order by c.id",
                    new {userId}))
            .ToArray();

        public async Task<UserFishModel> GetUserFish(long userId, long fishId)
        {
            // получаем рыбу пользователя
            var res = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserFishModel>(@"
                    select uf.*, f.name, f.rarity, f.seasons, f.price from user_fishes as uf
                        inner join fishes f
                            on f.id = uf.fish_id
                    where uf.user_id = @userId
                      and uf.fish_id = @fishId",
                    new {userId, fishId});

            // если рыба у пользователя есть - возвращаем ее
            if (res != null) return res;

            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем рыбу
            var fish = await _fishService.GetFish(fishId);
            // выводим ошибку
            await Task.FromException(new Exception(IzumiNullableMessage.UserInventory.Parse(
                emotes.GetEmoteOrBlank(fish.Name), _local.Localize(fish.Name, 2))));

            return new UserFishModel();
        }

        public async Task<UserFishModel[]> GetUserFish(long userId) =>
            (await _con.GetConnection()
                .QueryAsync<UserFishModel>(@"
                    select uf.*, f.name, f.rarity, f.seasons, f.price from user_fishes as uf
                        inner join fishes f
                            on f.id = uf.fish_id
                    where uf.user_id = @userId
                    order by f.id",
                    new {userId}))
            .ToArray();

        public async Task<UserFoodModel> GetUserFood(long userId, long foodId)
        {
            // получаем блюдо пользователя
            var res = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserFoodModel>(@"
                    select uf.*, f.name, f.mastery from user_foods as uf
                        inner join foods f
                            on f.id = uf.food_id
                    where uf.user_id = @userId
                      and uf.food_id = @foodId",
                    new {userId, foodId});

            // если блюдо у пользователя есть - возвращаем его
            if (res != null) return res;

            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем блюдо
            var food = await _foodService.GetFood(foodId);
            // выводим ошибку
            await Task.FromException(new Exception(IzumiNullableMessage.UserInventory.Parse(
                emotes.GetEmoteOrBlank(food.Name), _local.Localize(food.Name, 2))));

            return new UserFoodModel();
        }

        public async Task<UserFoodModel[]> GetUserFood(long userId) =>
            (await _con.GetConnection()
                .QueryAsync<UserFoodModel>(@"
                    select uf.*, f.name, f.mastery from user_foods as uf
                        inner join foods f
                            on f.id = uf.food_id
                    where uf.user_id = @userId
                    order by f.id",
                    new {userId}))
            .ToArray();

        public async Task<UserCurrencyModel> GetUserCurrency(long userId, Currency currency)
        {
            // получаем валюту пользователя
            var res = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserCurrencyModel>(@"
                    select * from user_currencies
                    where user_id = @userId
                      and currency = @currency",
                    new {userId, currency});

            // если валюта у пользователя есть - возвращаем ее
            if (res != null) return res;

            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // выводим ошибку
            await Task.FromException(new Exception(IzumiNullableMessage.UserInventory.Parse(
                emotes.GetEmoteOrBlank(currency.ToString()), _local.Localize(currency.ToString(), 2))));

            return new UserCurrencyModel();
        }

        public async Task<Dictionary<Currency, UserCurrencyModel>> GetUserCurrency(long userId) =>
            (await _con.GetConnection()
                .QueryAsync<UserCurrencyModel>(@"
                    select * from user_currencies
                    where user_id = @userId
                    order by currency",
                    new {userId}))
            .ToDictionary(x => x.Currency);

        public async Task<UserBoxModel> GetUserBox(long userId, Box box)
        {
            // получаем коробку пользователя
            var res = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserBoxModel>(@"
                    select * from user_boxes
                    where user_id = @userId
                      and box = @box",
                    new {userId, box});

            // если коробка у пользователя есть - возвращаем ее
            if (res != null) return res;

            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // выводим ошибку
            await Task.FromException(new Exception(IzumiNullableMessage.UserInventory.Parse(
                emotes.GetEmoteOrBlank(box.Emote()), _local.Localize(box.ToString()))));

            return new UserBoxModel();
        }

        public async Task<Dictionary<Box, UserBoxModel>> GetUserBox(long userId) =>
            (await _con.GetConnection()
                .QueryAsync<UserBoxModel>(@"
                    select * from user_boxes
                    where user_id = @userId
                    order by box",
                    new {userId}))
            .ToDictionary(x => x.Box);

        public async Task<bool> CheckItemInUser(long userId, InventoryCategory category, long itemId)
        {
            // запрос в базу зависит от категории предмета
            var query = category switch
            {
                InventoryCategory.Currency => @"
                    select 1 from user_currencies
                    where user_id = @userId
                      and currency = @itemId",

                InventoryCategory.Gathering => @"
                    select 1 from user_gatherings
                    where user_id = @userId
                      and gathering_id = @itemId",

                InventoryCategory.Product => @"
                    select 1 from user_products
                    where user_id = @userId
                      and product_id = @itemId",

                InventoryCategory.Crafting => @"
                    select 1 from user_craftings
                    where user_id = @userId
                      and crafting_id = @itemId",

                InventoryCategory.Alcohol => @"
                    select 1 from user_alcohols
                    where user_id = @userId
                      and alcohol_id = @itemId",

                InventoryCategory.Drink => @"
                    select 1 from user_drinks
                    where user_id = @userId
                      and drink_id = @itemId",

                InventoryCategory.Seed => @"
                    select 1 from user_seeds
                    where user_id = @userId
                      and seed_id = @itemId",

                InventoryCategory.Crop => @"
                    select 1 from user_crops
                    where user_id = @userId
                      and crop_id = @itemId",

                InventoryCategory.Fish => @"
                    select 1 from user_fishes
                    where user_id = @userId
                      and fish_id = @itemId",

                InventoryCategory.Food => @"
                    select 1 from user_foods
                    where user_id = @userId
                      and food_id = @itemId",

                InventoryCategory.Box => @"
                    select 1 from user_boxes
                    where user_id = @userId
                      and box = @itemId",

                _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
            };

            // проверяем наш запрос и возвращаем результат
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<bool>(query,
                    new {userId, itemId});
        }

        public async Task AddItemToUser(long userId, InventoryCategory category, long itemId, long amount = 1)
        {
            // запрос в базу зависит от категории предмета
            var query = category switch
            {
                InventoryCategory.Currency => @"
                        insert into user_currencies as u (user_id, currency, amount)
                        values (@userId, @itemId, @amount)
                        on conflict (user_id, currency) do update
                            set amount = u.amount + @amount,
                                updated_at = now()",

                InventoryCategory.Gathering => @"
                        insert into user_gatherings as u (user_id, gathering_id, amount)
                        values (@userId, @itemId, @amount)
                        on conflict (user_id, gathering_id) do update
                            set amount = u.amount + @amount,
                                updated_at = now()",

                InventoryCategory.Product => @"
                        insert into user_products as u (user_id, product_id, amount)
                        values (@userId, @itemId, @amount)
                        on conflict (user_id, product_id) do update
                            set amount = u.amount + @amount,
                                updated_at = now()",

                InventoryCategory.Crafting => @"
                        insert into user_craftings as u (user_id, crafting_id, amount)
                        values (@userId, @itemId, @amount)
                        on conflict (user_id, crafting_id) do update
                            set amount = u.amount + @amount,
                                updated_at = now()",

                InventoryCategory.Alcohol => @"
                        insert into user_alcohols as u (user_id, alcohol_id, amount)
                        values (@userId, @itemId, @amount)
                        on conflict (user_id, alcohol_id) do update
                            set amount = u.amount + @amount,
                                updated_at = now()",

                InventoryCategory.Drink => @"
                        insert into user_drinks as u (user_id, drink_id, amount)
                        values (@userId, @itemId, @amount)
                        on conflict (user_id, drink_id) do update
                            set amount = u.amount + @amount,
                                updated_at = now()",

                InventoryCategory.Seed => @"
                        insert into user_seeds as u (user_id, seed_id, amount)
                        values (@userId, @itemId, @amount)
                        on conflict (user_id, seed_id) do update
                            set amount = u.amount + @amount,
                                updated_at = now()",

                InventoryCategory.Crop => @"
                        insert into user_crops as u (user_id, crop_id, amount)
                        values (@userId, @itemId, @amount)
                        on conflict (user_id, crop_id) do update
                            set amount = u.amount + @amount,
                                updated_at = now()",

                InventoryCategory.Fish => @"
                        insert into user_fishes as u (user_id, fish_id, amount)
                        values (@userId, @itemId, @amount)
                        on conflict (user_id, fish_id) do update
                            set amount = u.amount + @amount,
                                updated_at = now()",

                InventoryCategory.Food => @"
                        insert into user_foods as u (user_id, food_id, amount)
                        values (@userId, @itemId, @amount)
                        on conflict (user_id, food_id) do update
                            set amount = u.amount + @amount,
                                updated_at = now()",

                InventoryCategory.Box => @"
                    insert into user_boxes as u (user_id, box, amount)
                        values (@userId, @itemId, @amount)
                        on conflict (user_id, box) do update
                            set amount = u.amount + @amount,
                                updated_at = now()",

                _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
            };

            // отправляем наш запрос в базу
            await _con
                .GetConnection()
                .ExecuteAsync(query,
                    new {userId, itemId, amount});

            // если категория предмета это валюта, то нужно добавить статистику пользователю
            if (category == InventoryCategory.Currency)
                await _statisticService.AddStatisticToUser(userId, Statistic.CurrencyEarned, amount);
        }

        public async Task AddItemToUser(long userId, MarketCategory category, long itemId, long amount = 1) =>
            await AddItemToUser(userId, category switch
            {
                // переопределяем категорию
                MarketCategory.Gathering => InventoryCategory.Gathering,
                MarketCategory.Crafting => InventoryCategory.Crafting,
                MarketCategory.Alcohol => InventoryCategory.Alcohol,
                MarketCategory.Drink => InventoryCategory.Drink,
                MarketCategory.Food => InventoryCategory.Food,
                _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
            }, itemId, amount);

        public async Task AddItemToUser(long[] usersId, InventoryCategory category, long itemId, long amount = 1)
        {
            // запрос в базу зависит от категории предмета
            var query = category switch
            {
                InventoryCategory.Currency => @"
                    insert into user_currencies as u (user_id, currency, amount)
                    values (unnest(array[@usersId]), @itemId, @amount)
                    on conflict (user_id, currency) do update
                        set amount = u.amount + @amount,
                            updated_at = now()",

                InventoryCategory.Gathering => @"
                    insert into user_gatherings as u (user_id, gathering_id, amount)
                    values (unnest(array[@usersId]), @itemId, @amount)
                    on conflict (user_id, gathering_id) do update
                        set amount = u.amount + @amount,
                            updated_at = now()",

                InventoryCategory.Product => @"
                    insert into user_products as u (user_id, product_id, amount)
                    values (unnest(array[@usersId]), @itemId, @amount)
                    on conflict (user_id, product_id) do update
                        set amount = u.amount + @amount,
                            updated_at = now()",

                InventoryCategory.Crafting => @"
                    insert into user_craftings as u (user_id, crafting_id, amount)
                    values (unnest(array[@usersId]), @itemId, @amount)
                    on conflict (user_id, crafting_id) do update
                        set amount = u.amount + @amount,
                            updated_at = now()",

                InventoryCategory.Alcohol => @"
                    insert into user_alcohols as u (user_id, alcohol_id, amount)
                    values (unnest(array[@usersId]), @itemId, @amount)
                    on conflict (user_id, alcohol_id) do update
                        set amount = u.amount + @amount,
                            updated_at = now()",

                InventoryCategory.Drink => @"
                    insert into user_drinks as u (user_id, drink_id, amount)
                    values (unnest(array[@usersId]), @itemId, @amount)
                    on conflict (user_id, drink_id) do update
                        set amount = u.amount + @amount,
                            updated_at = now()",

                InventoryCategory.Seed => @"
                    insert into user_seeds as u (user_id, seed_id, amount)
                    values (unnest(array[@usersId]), @itemId, @amount)
                    on conflict (user_id, seed_id) do update
                        set amount = u.amount + @amount,
                            updated_at = now()",

                InventoryCategory.Crop => @"
                    insert into user_crops as u (user_id, crop_id, amount)
                    values (unnest(array[@usersId]), @itemId, @amount)
                    on conflict (user_id, crop_id) do update
                        set amount = u.amount + @amount,
                            updated_at = now()",

                InventoryCategory.Fish => @"
                    insert into user_fishes as u (user_id, fish_id, amount)
                    values (unnest(array[@usersId]), @itemId, @amount)
                    on conflict (user_id, fish_id) do update
                        set amount = u.amount + @amount,
                            updated_at = now()",

                InventoryCategory.Food => @"
                    insert into user_foods as u (user_id, food_id, amount)
                    values (unnest(array[@usersId]), @itemId, @amount)
                    on conflict (user_id, food_id) do update
                        set amount = u.amount + @amount,
                            updated_at = now()",

                InventoryCategory.Box => @"
                    insert into user_boxes as u (user_id, box, amount)
                    values (unnest(array[@usersId]), @itemId, @amount)
                    on conflict (user_id, box) do update
                        set amount = u.amount + @amount,
                            updated_at = now()",

                _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
            };

            // отправляем наш запрос в базу
            await _con
                .GetConnection()
                .ExecuteAsync(query,
                    new {usersId, itemId, amount});

            // если категория предмета это валюта, то нужно добавить пользователю статистику
            if (category == InventoryCategory.Currency)
                await _statisticService.AddStatisticToUser(usersId, Statistic.CurrencyEarned, amount);
        }

        public async Task RemoveItemFromUser(long userId, InventoryCategory category, long itemId, long amount = 1)
        {
            // запрос в базу зависит от категории предмета
            var query = category switch
            {
                InventoryCategory.Currency => @"
                    update user_currencies
                    set amount = amount - @amount,
                        updated_at = now()
                    where user_id = @userId
                      and currency = @itemId",

                InventoryCategory.Gathering => @"
                    update user_gatherings
                    set amount = amount - @amount,
                        updated_at = now()
                    where user_id = @userId
                      and gathering_id = @itemId",

                InventoryCategory.Product => @"
                    update user_products
                    set amount = amount - @amount,
                        updated_at = now()
                    where user_id = @userId
                      and product_id = @itemId",

                InventoryCategory.Crafting => @"
                    update user_craftings
                    set amount = amount - @amount,
                        updated_at = now()
                    where user_id = @userId
                      and crafting_id = @itemId",

                InventoryCategory.Alcohol => @"
                    update user_alcohols
                    set amount = amount - @amount,
                        updated_at = now()
                    where user_id = @userId
                      and alcohol_id = @itemId",

                InventoryCategory.Drink => @"
                    update user_drinks
                    set amount = amount - @amount,
                        updated_at = now()
                    where user_id = @userId
                      and drink_id = @itemId",

                InventoryCategory.Seed => @"
                    update user_seeds
                    set amount = amount - @amount,
                        updated_at = now()
                    where user_id = @userId
                      and seed_id = @itemId",

                InventoryCategory.Crop => @"
                    update user_crops
                    set amount = amount - @amount,
                        updated_at = now()
                    where user_id = @userId
                      and crop_id = @itemId",

                InventoryCategory.Fish => @"
                    update user_fishes
                    set amount = amount - @amount,
                        updated_at = now()
                    where user_id = @userId
                      and fish_id = @itemId",

                InventoryCategory.Food => @"
                    update user_foods
                    set amount = amount - @amount,
                        updated_at = now()
                    where user_id = @userId
                      and food_id = @itemId",

                InventoryCategory.Box => @"
                    update user_boxes
                    set amount = amount - @amount,
                        updated_at = now()
                    where user_id = @userId
                      and box = @itemId",

                _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
            };

            // отправляем наш запрос в базу
            await _con
                .GetConnection()
                .ExecuteAsync(query,
                    new {userId, itemId, amount});

            // если категория предмета это валюта, то нужно добавить пользователю статистику
            if (category == InventoryCategory.Currency)
                await _statisticService.AddStatisticToUser(userId, Statistic.CurrencySpent, amount);
        }

        public async Task RemoveItemFromUser(long userId, MarketCategory category, long itemId, long amount = 1) =>
            await RemoveItemFromUser(userId, category switch
            {
                // переопределяем категорию
                MarketCategory.Gathering => InventoryCategory.Gathering,
                MarketCategory.Crafting => InventoryCategory.Crafting,
                MarketCategory.Alcohol => InventoryCategory.Alcohol,
                MarketCategory.Drink => InventoryCategory.Drink,
                MarketCategory.Food => InventoryCategory.Food,
                _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
            }, itemId, amount);

        public async Task RemoveItemFromUser(long userId, IngredientCategory category, long itemId, long amount = 1) =>
            await RemoveItemFromUser(userId, category switch
            {
                // переопределяем категорию
                IngredientCategory.Gathering => InventoryCategory.Gathering,
                IngredientCategory.Product => InventoryCategory.Product,
                IngredientCategory.Crafting => InventoryCategory.Crafting,
                IngredientCategory.Alcohol => InventoryCategory.Alcohol,
                IngredientCategory.Drink => InventoryCategory.Drink,
                IngredientCategory.Crop => InventoryCategory.Crop,
                IngredientCategory.Food => InventoryCategory.Food,
                _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
            }, itemId, amount);
    }
}
