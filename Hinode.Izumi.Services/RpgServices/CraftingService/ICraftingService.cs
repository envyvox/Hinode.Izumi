using System.Threading.Tasks;
using Hinode.Izumi.Services.RpgServices.CraftingService.Models;

namespace Hinode.Izumi.Services.RpgServices.CraftingService
{
    public interface ICraftingService
    {
        /// <summary>
        /// Возвращает массив изготавливаемых предметов.
        /// </summary>
        /// <returns>Массив изготавливаемых предметов.</returns>
        Task<CraftingModel[]> GetAllCraftings();

        /// <summary>
        /// Возвращает изготавливаемый предмет. Кэшируется.
        /// </summary>
        /// <param name="id">Id изготавливаемого предмета.</param>
        /// <returns>Изготавливаемый предмет.</returns>
        Task<CraftingModel> GetCrafting(long id);
    }
}
