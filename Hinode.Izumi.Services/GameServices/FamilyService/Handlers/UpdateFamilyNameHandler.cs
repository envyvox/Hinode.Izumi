using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.FamilyService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Handlers
{
    public class UpdateFamilyNameHandler : IRequestHandler<UpdateFamilyNameCommand>
    {
        private readonly IConnectionManager _con;

        public UpdateFamilyNameHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(UpdateFamilyNameCommand request, CancellationToken cancellationToken)
        {
            var (familyId, newName) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    update families
                    set name = @newName,
                        updated_at = now()
                    where id = @familyId",
                    new {familyId, newName});

            return new Unit();
        }
    }
}
