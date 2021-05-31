using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.DiscordServices.DiscordRoleService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordRoleService.Commands
{
    public record AddDiscordUserRoleToDbCommand(long UserId, long RoleId, long Days) : IRequest;

    public class AddDiscordUserRoleToDbHandler : IRequestHandler<AddDiscordUserRoleToDbCommand>
    {
        private readonly IConnectionManager _con;
        private readonly IMediator _mediator;

        public AddDiscordUserRoleToDbHandler(IConnectionManager con, IMediator mediator)
        {
            _con = con;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(AddDiscordUserRoleToDbCommand request, CancellationToken ct)
        {
            var (userId, roleId, days) = request;
            var userRole = await _mediator.Send(new GetDiscordUserRoleQuery(userId, roleId), ct);

            string query;
            DateTimeOffset expiration;

            if (userRole is null)
            {
                expiration = DateTimeOffset.Now.AddDays(days);
                query = @"
                    insert into user_roles(user_id, role_id, expiration)
                    values (@userId, @roleId, @expiration)";
            }
            else
            {
                expiration = userRole.Expiration.AddDays(days);
                query = @"
                    update user_roles
                    set expiration = @expiration,
                        updated_at = now()
                    where user_id = @userId
                      and role_id = @roleId";
            }

            await _con.GetConnection()
                .ExecuteAsync(query,
                    new {userId, roleId, expiration});

            return new Unit();
        }
    }
}
