using System;
using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.GameServices.PremiumService.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hinode.Izumi.Services.WebServices.CommandWebService.Attributes
{
    public class RequirePremium : PreconditionAttribute
    {
        public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context,
            CommandInfo command, IServiceProvider services)
        {
            var hasPremium = await services.GetService<IMediator>()
                .Send(new CheckUserHasPremiumQuery((long) context.User.Id));

            return await Task.FromResult(hasPremium
                ? PreconditionResult.FromSuccess()
                : PreconditionResult.FromError(IzumiPreconditionErrorMessage.RequirePremium.Parse()));
        }
    }
}
