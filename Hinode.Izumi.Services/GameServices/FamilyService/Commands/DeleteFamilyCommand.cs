using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Commands
{
    public record DeleteFamilyCommand(long FamilyId) : IRequest;

    public class DeleteFamilyHandler : IRequestHandler<DeleteFamilyCommand>
    {
        private readonly IConnectionManager _con;

        public DeleteFamilyHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(DeleteFamilyCommand request, CancellationToken cancellationToken)
        {
            await _con.GetConnection()
                .ExecuteAsync(@"
                    delete from families
                    where id = @id",
                    new {id = request.FamilyId});

            return new Unit();
        }
    }
}
