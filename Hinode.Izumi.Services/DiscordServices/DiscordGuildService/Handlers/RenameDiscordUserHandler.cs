using System;
using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Handlers
{
    public class RenameDiscordUserHandler : IRequestHandler<RenameDiscordUserCommand>
    {
        private readonly IMediator _mediator;

        public RenameDiscordUserHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(RenameDiscordUserCommand request, CancellationToken cancellationToken)
        {
            var (id, newName) = request;
            var socketGuild = await _mediator.Send(new GetDiscordSocketGuildQuery(), cancellationToken);
            var socketGuildUser = socketGuild.GetUser((ulong) id);

            try
            {
                await socketGuildUser.ModifyAsync(x => x.Nickname = newName + " 🌺");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return new Unit();
        }
    }
}
