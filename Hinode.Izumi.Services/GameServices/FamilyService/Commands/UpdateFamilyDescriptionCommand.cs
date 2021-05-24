using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Commands
{
    public record UpdateFamilyDescriptionCommand(long FamilyId, string NewDescription) : IRequest;

    public class UpdateFamilyDescriptionHandler : IRequestHandler<UpdateFamilyDescriptionCommand>
    {
        private readonly IConnectionManager _con;

        public UpdateFamilyDescriptionHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(UpdateFamilyDescriptionCommand request, CancellationToken cancellationToken)
        {
            var (familyId, newDescription) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    update families
                    set description = @newDescription,
                        updated_at = now()
                    where id = @familyId",
                    new {familyId, newDescription});

            return new Unit();
        }
    }
}
