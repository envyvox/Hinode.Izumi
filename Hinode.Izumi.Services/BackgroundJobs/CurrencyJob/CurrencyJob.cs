using System.Linq;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.EffectEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.GameServices.EffectService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.BackgroundJobs.CurrencyJob
{
    [InjectableService]
    public class CurrencyJob : ICurrencyJob
    {
        private readonly IMediator _mediator;

        public CurrencyJob(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task DailyIncome()
        {
            // получаем пользователей с эффектом получения ежедневных иен
            var users = await _mediator.Send(new GetUsersWithEffectQuery(Effect.DailyIenIncome));
            // получаем массив с их Id
            var usersId = users.Select(x => x.Id).ToArray();
            // получаем количество ежедневных иен
            var amount = await _mediator.Send(new GetPropertyValueQuery(Property.EconomyDailyIncome));
            // добавляем иены всем подходящим пользователям
            await _mediator.Send(new AddItemToUsersByInventoryCategoryCommand(
                usersId, InventoryCategory.Currency, Currency.Ien.GetHashCode(), amount));
        }
    }
}
