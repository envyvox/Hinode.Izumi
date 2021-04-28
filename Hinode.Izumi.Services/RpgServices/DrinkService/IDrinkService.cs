using System.Threading.Tasks;
using Hinode.Izumi.Services.RpgServices.DrinkService.Models;

namespace Hinode.Izumi.Services.RpgServices.DrinkService
{
    public interface IDrinkService
    {
        /// <summary>
        /// Возвращает массив напитков.
        /// </summary>
        /// <returns>Массив напитков.</returns>
        Task<DrinkModel[]> GetAllDrinks();

        /// <summary>
        /// Возвращает напиток. Кэшируется.
        /// </summary>
        /// <param name="id">Id напитка.</param>
        /// <returns>Напиток.</returns>
        Task<DrinkModel> GetDrink(long id);
    }
}
