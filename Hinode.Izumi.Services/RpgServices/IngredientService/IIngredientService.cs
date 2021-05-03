using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.RpgServices.IngredientService.Models;

namespace Hinode.Izumi.Services.RpgServices.IngredientService
{
    public interface IIngredientService
    {
        /// <summary>
        /// Возвращает массив ингредиентов для изготовливаемого предмета.
        /// </summary>
        /// <param name="craftingId">Id изготавливаемого предмета.</param>
        /// <returns>Массив ингредиентов для изготовливаемого предмета.</returns>
        Task<CraftingIngredientModel[]> GetCraftingIngredients(long craftingId);

        /// <summary>
        /// Возвращает массив ингредиентов для алкоголя.
        /// </summary>
        /// <param name="alcoholId">Id алкоголя.</param>
        /// <returns>Массив ингредиентов для алкоголя.</returns>
        Task<AlcoholIngredientModel[]> GetAlcoholIngredients(long alcoholId);

        /// <summary>
        /// Возвращает массив ингредиентов для напитка.
        /// </summary>
        /// <param name="drinkId">Id напитка.</param>
        /// <returns>Массив ингредиентов для напитка.</returns>
        Task<DrinkIngredientModel[]> GetDrinkIngredients(long drinkId);

        /// <summary>
        /// Возвращает массив ингредиентов для блюда.
        /// </summary>
        /// <param name="foodId">Id блюда.</param>
        /// <returns>Массив ингредиентов для блюда.</returns>
        Task<FoodIngredientModel[]> GetFoodIngredients(long foodId);

        /// <summary>
        /// Возващает массив ингрендиентов для строительства.
        /// </summary>
        /// <param name="projectId">Id чертежа.</param>
        /// <returns>Массив ингрендиентов для строительства.</returns>
        Task<ProjectIngredientModel[]> GetProjectIngredients(long projectId);

        /// <summary>
        /// Возвращает массив сезонов блюда.
        /// </summary>
        /// <param name="foodId">Id блюда.</param>
        /// <returns>Массив сезонов.</returns>
        Task<List<Season>> GetFoodSeasons(long foodId);

        /// <summary>
        /// Возвращает себестоимость изготавливаемого предмета.
        /// </summary>
        /// <param name="craftingId">Id изготавливаемого предмета.</param>
        /// <returns>Себестоимость изготавливаемого предмета.</returns>
        Task<long> GetCraftingCostPrice(long craftingId);

        /// <summary>
        /// Возвращает себестоимость алкоголя.
        /// </summary>
        /// <param name="alcoholId">Id алкоголя.</param>
        /// <returns>Себестоимость алкоголя.</returns>
        Task<long> GetAlcoholCostPrice(long alcoholId);

        /// <summary>
        /// Возвращает себестоимость напитка.
        /// </summary>
        /// <param name="drinkId">Id напитка.</param>
        /// <returns>Себестоимость напитка.</returns>
        Task<long> GetDrinkCostPrice(long drinkId);

        /// <summary>
        /// Возвращает себестоимость блюда.
        /// </summary>
        /// <param name="foodId">Id блюда.</param>
        /// <returns>Себестоимость блюда.</returns>
        Task<long> GetFoodCostPrice(long foodId);

        /// <summary>
        /// Возвращает себестоимость чертежа.
        /// </summary>
        /// <param name="projectId">Id чертежа.</param>
        /// <returns>Себестоимость чертежа.</returns>
        Task<long> GetProjectCostPrice(long projectId);

        /// <summary>
        /// Возвращает локализированную строку с информацией об ингредиентах необходимых для изготовливаемого предемета.
        /// </summary>
        /// <param name="craftingId">Id изготавливаемого предмета.</param>
        /// <param name="amount">Количество изготовлений.</param>
        /// <returns>Локализированную строку с информацией об ингредиентах.</returns>
        Task<string> DisplayCraftingIngredients(long craftingId, long amount = 1);

        /// <summary>
        /// Возвращает локализированную строку с информацией об ингредиентах необходимых для алкоголя.
        /// </summary>
        /// <param name="alcoholId">Id алкоголя.</param>
        /// <param name="amount">Количество изготовлений.</param>
        /// <returns>Локализированную строку с информацией об ингредиентах.</returns>
        Task<string> DisplayAlcoholIngredients(long alcoholId, long amount = 1);

        /// <summary>
        /// Возвращает локализированную строку с информацией об ингредиентах необходимых для напитка.
        /// </summary>
        /// <param name="drinkId">Id напитка.</param>
        /// <param name="amount">Количество изготовлений.</param>
        /// <returns>Локализированную строку с информацией об ингредиентах.</returns>
        Task<string> DisplayDrinkIngredients(long drinkId, long amount = 1);

        /// <summary>
        /// Возвращает локализированную строку с информацией об ингредиентах необходимых для блюда.
        /// </summary>
        /// <param name="foodId">Id блюда.</param>
        /// <param name="amount">Количество изготовлений.</param>
        /// <returns>Локализированную строку с информацией об ингредиентах.</returns>
        Task<string> DisplayFoodIngredients(long foodId, long amount = 1);

        /// <summary>
        /// Возвращает локализированную строку с информацией об ингредиентах необходимых для строительства.
        /// </summary>
        /// <param name="projectId">Id чертежа.</param>
        /// <returns>Локализированную строку с информацией об ингредиентах.</returns>
        Task<string> DisplayProjectIngredients(long projectId);

        /// <summary>
        /// Проверяет наличие всех необходимых ингредиентов для изготавливаемого предмета в инвентаре пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="craftingId">Id изготавливаемого предмета.</param>
        /// <param name="amount">Количество изготовлений.</param>
        Task CheckCraftingIngredients(long userId, long craftingId, long amount = 1);

        /// <summary>
        /// Проверяет наличие всех необходимых ингредиентов для алкоголя в инвентаре пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="alcoholId">Id алкоголя.</param>
        /// <param name="amount">Количество изготовлений.</param>
        Task CheckAlcoholIngredients(long userId, long alcoholId, long amount = 1);

        /// <summary>
        /// Проверяет наличие всех необходимых ингредиентов для напитка в инвентаре пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="drinkId">Id напитка.</param>
        /// <param name="amount">Количество изготовлений.</param>
        Task CheckDrinkIngredients(long userId, long drinkId, long amount = 1);

        /// <summary>
        /// Проверяет наличие всех необходимых ингредиентов для блюда в инвентаре пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="foodId">Id блюда.</param>
        /// <param name="amount">Количество изготовлений.</param>
        Task CheckFoodIngredients(long userId, long foodId, long amount = 1);

        /// <summary>
        /// Проверяет наличие всех необходимых ингредиентов для строительства в инвентаре пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="projectId">Id ингредиента.</param>
        Task CheckProjectIngredients(long userId, long projectId);

        /// <summary>
        /// Отнимает из инвентаря пользователя все необходимые ингредиенты для изготавливаемого предмета.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="craftingId">Id изготавливаемого предмета.</param>
        /// <param name="amount">Количество изготовлений.</param>
        Task RemoveCraftingIngredients(long userId, long craftingId, long amount = 1);

        /// <summary>
        /// Отнимает из инвентаря пользователя все необходимые ингредиенты для алкоголя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="alcoholId">Id алкоголя.</param>
        /// <param name="amount">Количество изготовлений.</param>
        Task RemoveAlcoholIngredients(long userId, long alcoholId, long amount = 1);

        /// <summary>
        /// Отнимает из инвентаря пользователя все необходимые ингредиенты для напитка.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="drinkId">Id напитка.</param>
        /// <param name="amount">Количество изготовлений.</param>
        Task RemoveDrinkIngredients(long userId, long drinkId, long amount = 1);

        /// <summary>
        /// Отнимает из инвентаря пользователя все необходимые ингредиенты для блюда.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="foodId">Id блюда.</param>
        /// <param name="amount">Количество приготовлений.</param>
        Task RemoveFoodIngredients(long userId, long foodId, long amount = 1);

        /// <summary>
        /// Отнимает из инвентаря пользователя все необходимые ингредиенты для строительства.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="projectId">Id чертежа.</param>
        /// <returns></returns>
        Task RemoveProjectIngredients(long userId, long projectId);
    }
}
