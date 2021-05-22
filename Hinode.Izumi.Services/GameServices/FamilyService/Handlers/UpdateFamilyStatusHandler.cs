using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.FamilyService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Handlers
{
    public class UpdateFamilyStatusHandler : IRequestHandler<UpdateFamilyStatusCommand>
    {
        private readonly IConnectionManager _con;

        public UpdateFamilyStatusHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(UpdateFamilyStatusCommand request, CancellationToken cancellationToken)
        {
            var (familyId, newStatus) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    update families
                    set status = @newStatus,
                        updated_at = now()
                    where id = @familyId",
                    new {familyId, newStatus});

            return new Unit();
        }
    }
}
