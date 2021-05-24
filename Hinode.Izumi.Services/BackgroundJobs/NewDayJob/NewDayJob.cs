using System;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.GameServices.FieldService.Commands;
using Hinode.Izumi.Services.GameServices.PropertyService.Commands;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.BackgroundJobs.NewDayJob
{
    [InjectableService]
    public class NewDayJob : INewDayJob
    {
        private readonly IMediator _mediator;

        public NewDayJob(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task StartNewDay()
        {
            // получаем текущий дебафф мира
            var debuff = (BossDebuff) await _mediator.Send(new GetPropertyValueQuery(Property.BossDebuff));

            // если в деревне дебафа нет - нужно двинуть прогресс всех клеток земли участков
            if (debuff != BossDebuff.VillageStop) await _mediator.Send(new MoveAllFieldsProgressCommand());

            // генерируем погоду
            await GenerateWeather();

            // если в деревне нет дебафа - нужно обновить состояние всех клеток земли участков
            if (debuff != BossDebuff.VillageStop)
            {
                // получаем текущую погоду
                var weather = (Weather) await _mediator.Send(new GetPropertyValueQuery(Property.WeatherToday));
                // обновляем состояние клеток земли участков
                await _mediator.Send(new UpdateAllFieldsStateCommand(weather == Weather.Rain
                    // если идет дождь - ячейки должны быть политы
                    ? FieldState.Watered
                    : FieldState.Planted));
            }
        }

        private async Task GenerateWeather()
        {
            var chance = new Random().Next(1, 101);
            // запускается на новом дне, значит сегодняшняя погода теперь вчерашняя погода
            var yesterday = (Weather) await _mediator.Send(new GetPropertyValueQuery(Property.WeatherToday));
            // а завтрашняя погода теперь сегодняшняя погода
            var nToday = (Weather) await _mediator.Send(new GetPropertyValueQuery(Property.WeatherTomorrow));
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
            await _mediator.Send(new UpdatePropertyCommand(Property.WeatherToday, nToday.GetHashCode()));
            await _mediator.Send(new UpdatePropertyCommand(Property.WeatherTomorrow, nTomorrow.GetHashCode()));
        }
    }
}
