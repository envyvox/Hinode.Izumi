using System;
using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hinode.Izumi.Commands.Attributes
{
    public class IzumiRequireRole : PreconditionAttribute
    {
        private readonly DiscordRole _role;

        public IzumiRequireRole(DiscordRole role)
        {
            _role = role;
        }

        public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context,
            CommandInfo command, IServiceProvider services)
        {
            var mediator = services.GetService<IMediator>();
            var checkRole = await mediator.Send(
                new CheckDiscordRoleInUserQuery((long) context.User.Id, _role));
            var checkAdmin = await mediator.Send(
                new CheckDiscordRoleInUserQuery((long) context.User.Id, DiscordRole.Administration));

            return await Task.FromResult(checkRole
                ? PreconditionResult.FromSuccess()
                : checkAdmin
                    ? PreconditionResult.FromSuccess()
                    : PreconditionResult.FromError(IzumiPreconditionErrorMessage.RequireRole.Parse(_role.Name())));
        }
    }
}
