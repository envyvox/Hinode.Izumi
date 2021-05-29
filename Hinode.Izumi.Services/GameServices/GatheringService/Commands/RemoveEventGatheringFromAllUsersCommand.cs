using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.GatheringService.Commands
{
    public record RemoveEventGatheringFromAllUsersCommand(Event Event) : IRequest;

    public class RemoveEventGatheringFromAllUsersHandler : IRequestHandler<RemoveEventGatheringFromAllUsersCommand>
    {
        private readonly IConnectionManager _con;

        public RemoveEventGatheringFromAllUsersHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(RemoveEventGatheringFromAllUsersCommand request,
            CancellationToken cancellationToken)
        {
            await _con.GetConnection()
                .ExecuteAsync(@"
                    delete from user_gatherings
                    where gathering_id = (
                        select id from gatherings
                        where event = @event)",
                    new {@event = request.Event});

            return new Unit();
        }
    }
}
