using System;
using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hinode.Izumi.Services.WebServices.CommandWebService.Attributes
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
            var currentEvent = (Event) await services.GetService<IMediator>()
                .Send(new GetPropertyValueQuery(Property.CurrentEvent));

            return await Task.FromResult(currentEvent == _event
                ? PreconditionResult.FromSuccess()
                : PreconditionResult.FromError(IzumiPreconditionErrorMessage.RequireEvent.Parse(
                    _event.Localize())));
        }
    }
}
