using System.Threading.Tasks;
using Discord;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.BackgroundJobs.MuteJob
{
    [InjectableService]
    public class MuteJob : IMuteJob
    {
        private readonly IMediator _mediator;

        public MuteJob(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Unmute(long userId)
        {
            // снимаем роль блокировки чата с пользователя
            await _mediator.Send(new RemoveDiscordRoleFromUserCommand(userId, DiscordRole.Mute));

            var embed = new EmbedBuilder()
                // подтвержаем снятия блокировки чата
                .WithDescription(IzumiReplyMessage.UnmuteDesc.Parse());

            await _mediator.Send(new SendEmbedToUserCommand(
                await _mediator.Send(new GetDiscordSocketUserQuery(userId)), embed));
        }
    }
}
