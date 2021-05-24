using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Commands
{
    public record DeleteFamilyInviteCommand(long Id) : IRequest;

    public class DeleteFamilyInviteHandler : IRequestHandler<DeleteFamilyInviteCommand>
    {
        private readonly IConnectionManager _con;

        public DeleteFamilyInviteHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(DeleteFamilyInviteCommand request, CancellationToken cancellationToken)
        {
            await _con.GetConnection()
                .ExecuteAsync(@"
                    select * from family_invites
                    where id = @id",
                    new {id = request.Id});

            return new Unit();
        }
    }
}
