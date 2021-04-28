using System;
using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.RpgServices.PropertyService;
using Microsoft.Extensions.DependencyInjection;

namespace Hinode.Izumi.Services.Commands.Attributes
{
    public class IzumiRequireEvent : PreconditionAttribute
    {
        private readonly Event _event;

        public IzumiRequireEvent(Event @event)
        {
            _event = @event;
        }

        public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context,
            CommandInfo command, IServiceProvider services)
        {
            // получаем текущий ивент
            var currentEvent = (Event) await services.GetService<IPropertyService>()
                .GetPropertyValue(Property.CurrentEvent);
            // возвращаем результат в зависимости от проверки
            return await Task.FromResult(currentEvent == _event
                // если текущий ивент это необходимый ивент - то все ок
                ? PreconditionResult.FromSuccess()
                // если нет - выводим ошибку
                : PreconditionResult.FromError(IzumiPreconditionErrorMessage.RequireEvent.Parse(
                    _event.Localize())));
        }
    }
}
