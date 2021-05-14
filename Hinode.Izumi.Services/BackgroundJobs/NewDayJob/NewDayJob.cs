using System;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.RpgServices.FieldService;
using Hinode.Izumi.Services.RpgServices.PropertyService;

namespace Hinode.Izumi.Services.BackgroundJobs.NewDayJob
{
    [InjectableService]
    public class NewDayJob : INewDayJob
    {
        private readonly IPropertyService _propertyService;
        private readonly IFieldService _fieldService;

        public NewDayJob(IPropertyService propertyService, IFieldService fieldService)
        {
            _propertyService = propertyService;
            _fieldService = fieldService;
        }

        public async Task StartNewDay()
        {
            // получаем текущий дебафф мира
            var debuff = (BossDebuff) await _propertyService.GetPropertyValue(Property.BossDebuff);

            // если в деревне дебафа нет - нужно двинуть прогресс всех клеток земли участков
            if (debuff != BossDebuff.VillageStop) await _fieldService.MoveProgress();

            // генерируем погоду
            await GenerateWeather();

            // если в деревне нет дебафа - нужно обновить состояние всех клеток земли участков
            if (debuff != BossDebuff.VillageStop)
            {
                // получаем текущую погоду
                var weather = (Weather) await _propertyService.GetPropertyValue(Property.WeatherToday);
                // обновляем состояние клеток земли участков
                await _fieldService.UpdateState(weather == Weather.Rain
                    // если идет дождь - ячейки должны быть политы
                    ? FieldState.Watered
                    : FieldState.Planted);
            }
        }

        private async Task GenerateWeather()
        {
            var chance = new Random().Next(1, 101);
            // запускается на новом дне, значит сегодняшняя погода теперь вчерашняя погода
            var yesterday = (Weather) await _propertyService.GetPropertyValue(Property.WeatherToday);
            // а завтрашняя погода теперь сегодняшняя погода
            var nToday = (Weather) await _propertyService.GetPropertyValue(Property.WeatherTomorrow);
            // и теперь нам остается сгенерировать погоду для нового завтра
            var nTomorrow =
                // если новая погода сегодня - ясная, значит завтра больше шанса для дождя и наоборот
                nToday == Weather.Clear
                    // однако если вчера погода так же была ясной - то завтра еще больше шанса для дождя и наоборот
                    ? chance + (yesterday == Weather.Rain ? 10 : 20) > 50
                        ? Weather.Rain
                        : Weather.Clear
                    : chance + (yesterday == Weather.Clear ? 10 : 20) > 50
                        ? Weather.Clear
                        : Weather.Rain;

            // обновляем значения в базе
            await _propertyService.UpdateProperty(Property.WeatherToday, nToday.GetHashCode());
            await _propertyService.UpdateProperty(Property.WeatherTomorrow, nTomorrow.GetHashCode());
        }
    }
}
