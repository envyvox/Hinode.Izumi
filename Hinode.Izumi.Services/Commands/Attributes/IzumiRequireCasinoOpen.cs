using System;
using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.RpgServices.PropertyService;
using Microsoft.Extensions.DependencyInjection;

namespace Hinode.Izumi.Services.Commands.Attributes
{
    public class IzumiRequireCasinoOpen : PreconditionAttribute
    {
        public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context,
            CommandInfo command, IServiceProvider services)
        {
            // получаем состояние казино
            var casinoState = await services.GetService<IPropertyService>().GetPropertyValue(Property.CasinoState);
            // если 1 то оно открыто, если 0 - закрыто
            var open = casinoState == 1;

            return await Task.FromResult(open
                ? PreconditionResult.FromSuccess()
                : PreconditionResult.FromError(IzumiPreconditionErrorMessage.RequireCasinoOpen.Parse()));
        }
    }
}
