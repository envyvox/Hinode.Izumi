using System.Threading.Tasks;
using Hinode.Izumi.Services.RpgServices.FoodService.Models;

namespace Hinode.Izumi.Services.RpgServices.FoodService
{
    public interface IFoodService
    {
        /// <summary>
        /// Возвращает массив блюд.
        /// </summary>
        /// <returns>Массив блюд.</returns>
        Task<FoodModel[]> GetAllFood();

        /// <summary>
        /// Возвращает блюдо. Кэшируется.
        /// </summary>
        /// <param name="id">Id блюда.</param>
        /// <returns>Блюдо.</returns>
        Task<FoodModel> GetFood(long id);

        /// <summary>
        /// Возвращает массив рецептов у пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Массив рецептов у пользователя.</returns>
        Task<FoodModel[]> GetUserRecipes(long userId);

        /// <summary>
        /// Проверяет есть ли у пользователя необходимый рецепт.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="foodId">Id блюда.</param>
        /// <returns>True если есть, false если нет.</returns>
        Task<bool> CheckUserRecipe(long userId, long foodId);

        /// <summary>
        /// Добавляет рецепт пользователю.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="foodId">Id блюда.</param>
        Task AddRecipeToUser(long userId, long foodId);
    }
}
