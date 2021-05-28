using System;
using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hinode.Izumi.Services.WebServices.CommandWebService.Attributes
{
    public class IzumiRequireCasinoOpen : PreconditionAttribute
    {
        public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context,
            CommandInfo command, IServiceProvider services)
        {
            var casinoState = await services.GetService<IMediator>()
                .Send(new GetPropertyValueQuery(Property.CasinoState));
            // если 1 то оно открыто, если 0 - закрыто
            var open = casinoState == 1;

            return await Task.FromResult(open
                ? PreconditionResult.FromSuccess()
                : PreconditionResult.FromError(IzumiPreconditionErrorMessage.RequireCasinoOpen.Parse()));
        }
    }
}
