using System.Linq;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.EffectEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.RpgServices.EffectService;
using Hinode.Izumi.Services.RpgServices.InventoryService;
using Hinode.Izumi.Services.RpgServices.PropertyService;

namespace Hinode.Izumi.Services.BackgroundJobs.CurrencyJob
{
    [InjectableService]
    public class CurrencyJob : ICurrencyJob
    {
        private readonly IEffectService _effectService;
        private readonly IInventoryService _inventoryService;
        private readonly IPropertyService _propertyService;

        public CurrencyJob(IEffectService effectService, IInventoryService inventoryService,
            IPropertyService propertyService)
        {
            _effectService = effectService;
            _inventoryService = inventoryService;
            _propertyService = propertyService;
        }


        public async Task DailyIncome()
        {
            // получаем пользователей с эффектом получения ежедневных иен
            var users = await _effectService.GetUsersWithEffect(Effect.DailyIenIncome);
            // получаем массив с их Id
            var usersId = users.Select(x => x.Id).ToArray();
            // получаем количество ежедневных иен
            var amount = await _propertyService.GetPropertyValue(Property.EconomyDailyIncome);
            // добавляем иены всем подходящим пользователям
            await _inventoryService.AddItemToUser(
                usersId, InventoryCategory.Currency, Currency.Ien.GetHashCode(), amount);
        }
    }
}
