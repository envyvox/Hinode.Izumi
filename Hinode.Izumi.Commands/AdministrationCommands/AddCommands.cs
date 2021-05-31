using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordRoleService.Commands;
using Hinode.Izumi.Services.GameServices.PremiumService.Commands;
using Hinode.Izumi.Services.WebServices.CommandWebService.Attributes;
using MediatR;

namespace Hinode.Izumi.Commands.AdministrationCommands
{
    [Group("add")]
    [IzumiRequireRole(DiscordRole.Administration)]
    public class AddRoleCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;
        private const string Ok = "👌";

        public AddRoleCommands(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Command("role")]
        public async Task AddRoleToUser(long userId, long roleId, long days)
        {
            await _mediator.Send(new AddDiscordRoleToUserByRoleIdCommand(userId, roleId));
            await _mediator.Send(new AddDiscordUserRoleToDbCommand(userId, roleId, days));
            await ReplyAsync(Ok);
            await Task.CompletedTask;
        }

        [Command("premium")]
        public async Task AddPremiumToUser(long userId, long days)
        {
            await _mediator.Send(new ActivateUserPremiumCommand(userId, days));
            await ReplyAsync(Ok);
            await Task.CompletedTask;
        }
    }
}
