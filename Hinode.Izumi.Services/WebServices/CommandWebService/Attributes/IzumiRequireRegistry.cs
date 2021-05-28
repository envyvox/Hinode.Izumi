using System;
using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.GameServices.UserService.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hinode.Izumi.Services.WebServices.CommandWebService.Attributes
{
    public class IzumiRequireRegistry : PreconditionAttribute
    {
        public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context,
            CommandInfo command, IServiceProvider services)
        {
            var checkUser = await services.GetService<IMediator>()
                .Send(new CheckUserByIdQuery((long) context.User.Id));

            return await Task.FromResult(checkUser
                ? PreconditionResult.FromSuccess()
                : PreconditionResult.FromError(IzumiPreconditionErrorMessage.RequireRegistry.Parse()));
        }
    }
}
