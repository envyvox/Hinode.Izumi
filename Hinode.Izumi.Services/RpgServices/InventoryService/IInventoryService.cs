using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.RpgServices.InventoryService.Models;

namespace Hinode.Izumi.Services.RpgServices.InventoryService
{
    public interface IInventoryService
    {
        /// <summary>
        /// Возвращает собирательский ресурс у пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="gatheringId">Id собирательского ресурса.</param>
        /// <returns>Собирательский ресурс у пользователя.</returns>
        /// <exception cref="IzumiNullableMessage.UserInventory"></exception>
        Task<UserGatheringModel> GetUserGathering(long userId, long gatheringId);

        /// <summary>
        /// Возвращает массив собирательских ресурсов у пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Массив собирательских ресурсов у пользователя.</returns>
        Task<UserGatheringModel[]> GetUserGathering(long userId);

        /// <summary>
        /// Возвращает продукт у пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="productId">Id продукта.</param>
        /// <returns></returns>
        /// <exception cref="IzumiNullableMessage.UserInventory"></exception>
        Task<UserProductModel> GetUserProduct(long userId, long productId);

        /// <summary>
        /// Возвращает массив продуктов у пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Массив продуктов у пользователя.</returns>
        Task<UserProductModel[]> GetUserProduct(long userId);

        /// <summary>
        /// Возвращает изготавливаемый предмет у пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="craftingId">Id изготавливаемого предмета.</param>
        /// <returns>Изготавливаемый предмет у пользователя.</returns>
        /// <exception cref="IzumiNullableMessage.UserInventory"></exception>
        Task<UserCraftingModel> GetUserCrafting(long userId, long craftingId);

        /// <summary>
        /// Возвращает массив изготавливаемых предметов у пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Массив изготавливаемых предметов у пользователя.</returns>
        Task<UserCraftingModel[]> GetUserCrafting(long userId);

        /// <summary>
        /// Возвращает алкоголь у пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="alcoholId">Id алкоголя.</param>
        /// <returns>Алкоголь у пользователя.</returns>
        /// <exception cref="IzumiNullableMessage.UserInventory"></exception>
        Task<UserAlcoholModel> GetUserAlcohol(long userId, long alcoholId);

        /// <summary>
        /// Возвращает массив алкоголя у пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Массив алкоголя у пользователя.</returns>
        Task<UserAlcoholModel[]> GetUserAlcohol(long userId);

        /// <summary>
        /// Возвращает напиток у пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="drinkId">Id напитка.</param>
        /// <returns>Напиток у пользователя.</returns>
        /// <exception cref="IzumiNullableMessage.UserInventory"></exception>
        Task<UserDrinkModel> GetUserDrink(long userId, long drinkId);

        /// <summary>
        /// Возвращает массив напитков у пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Массив напитков у пользователя.</returns>
        Task<UserDrinkModel[]> GetUserDrink(long userId);

        /// <summary>
        /// Возвращает семя у пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="seedId">Id семени.</param>
        /// <returns>Семя у пользователя.</returns>
        /// <exception cref="IzumiNullableMessage.UserInventory"></exception>
        Task<UserSeedModel> GetUserSeed(long userId, long seedId);

        /// <summary>
        /// Возвращает массив семян у пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Массив семян у пользователя.</returns>
        Task<UserSeedModel[]> GetUserSeed(long userId);

        /// <summary>
        /// Возвращает урожай у пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="cropId">Id урожая.</param>
        /// <returns>Урожай у пользователя.</returns>
        /// <exception cref="IzumiNullableMessage.UserInventory"></exception>
        Task<UserCropModel> GetUserCrop(long userId, long cropId);

        /// <summary>
        /// Возвращает массив урожаев у пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Массив урожаев у пользователя.</returns>
        Task<UserCropModel[]> GetUserCrop(long userId);

        /// <summary>
        /// Возвращает рыбу у пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="fishId">Id рыбы.</param>
        /// <returns>Рыба у пользователя.</returns>
        /// <exception cref="IzumiNullableMessage.UserInventory"></exception>
        Task<UserFishModel> GetUserFish(long userId, long fishId);

        /// <summary>
        /// Возвращает массив рыбы у пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Массив рыбы у пользователя.</returns>
        Task<UserFishModel[]> GetUserFish(long userId);

        /// <summary>
        /// Возвращает блюдо у пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="foodId">Id блюда.</param>
        /// <returns>Блюдо у пользователя.</returns>
        /// <exception cref="IzumiNullableMessage.UserInventory"></exception>
        Task<UserFoodModel> GetUserFood(long userId, long foodId);

        /// <summary>
        /// Возвращает массив блюд у пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Массив блюд у пользователя.</returns>
        Task<UserFoodModel[]> GetUserFood(long userId);

        /// <summary>
        /// Возвращает валюту у пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="currency">Валюта.</param>
        /// <returns>Валюта у пользователя.</returns>
        /// <exception cref="IzumiNullableMessage.UserInventory"></exception>
        Task<UserCurrencyModel> GetUserCurrency(long userId, Currency currency);

        /// <summary>
        /// Возвращает библитетку валют у пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Библиотека валют у пользователя.</returns>
        Task<Dictionary<Currency, UserCurrencyModel>> GetUserCurrency(long userId);

        /// <summary>
        /// Возвращает коробку у пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="box">Коробка.</param>
        /// <returns>Коробка у пользователя.</returns>
        /// <exception cref="IzumiNullableMessage.UserInventory"></exception>
        Task<UserBoxModel> GetUserBox(long userId, Box box);

        /// <summary>
        /// Возвращает библиотеку коробок у пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Библиотека коробок у пользователя.</returns>
        Task<Dictionary<Box, UserBoxModel>> GetUserBox(long userId);

        /// <summary>
        /// Проверяет наличие предмета в инвентаре пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="category">Категория инвентаря.</param>
        /// <param name="itemId">Id предмета.</param>
        /// <returns>True если есть, false если нет.</returns>
        Task<bool> CheckItemInUser(long userId, InventoryCategory category, long itemId);

        /// <summary>
        /// Добавляет предмет в инвентарь пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="category">Категория инвентаря.</param>
        /// <param name="itemId">Id предмета.</param>
        /// <param name="amount">Количество предметов.</param>
        Task AddItemToUser(long userId, InventoryCategory category, long itemId, long amount = 1);

        /// <summary>
        /// Добавляет предмет в инвентарь пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="category">Категория товара.</param>
        /// <param name="itemId">Id предмета.</param>
        /// <param name="amount">Количество предметов.</param>
        Task AddItemToUser(long userId, MarketCategory category, long itemId, long amount = 1);

        /// <summary>
        /// Добавляет предмет в инвентарь массива пользователей.
        /// </summary>
        /// <param name="usersId"></param>
        /// <param name="category">Категория инвентаря.</param>
        /// <param name="itemId">Id предмета.</param>
        /// <param name="amount">Количество предметов.</param>
        Task AddItemToUser(long[] usersId, InventoryCategory category, long itemId, long amount = 1);

        /// <summary>
        /// Отнимает предмет из инвентаря пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="category">Категория инвентаря.</param>
        /// <param name="itemId">Id предмета.</param>
        /// <param name="amount">Количество предметов.</param>
        Task RemoveItemFromUser(long userId, InventoryCategory category, long itemId, long amount = 1);

        /// <summary>
        /// Отнимает предмет из инвентаря пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="category">Категория товара.</param>
        /// <param name="itemId">Id предмета.</param>
        /// <param name="amount">Количество предметов.</param>
        Task RemoveItemFromUser(long userId, MarketCategory category, long itemId, long amount = 1);

        /// <summary>
        /// Отнимает предмет из инвентаря пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="category">Категория ингредиента.</param>
        /// <param name="itemId">Id предмета.</param>
        /// <param name="amount">Количество предметов.</param>
        Task RemoveItemFromUser(long userId, IngredientCategory category, long itemId, long amount = 1);
    }
}
