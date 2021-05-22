using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.EffectService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.EffectService.Handlers
{
    public class RemoveEffectFromUserHandler : IRequestHandler<RemoveEffectFromUserCommand>
    {
        private readonly IConnectionManager _con;

        public RemoveEffectFromUserHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(RemoveEffectFromUserCommand request, CancellationToken cancellationToken)
        {
            var (userId, effect) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    delete from user_effects
                    where user_id = @userId
                      and effect = @effect",
                    new {userId, effect});

            return new Unit();
        }
    }
}
