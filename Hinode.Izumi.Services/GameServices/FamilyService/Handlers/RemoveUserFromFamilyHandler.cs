using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.FamilyService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Handlers
{
    public class RemoveUserFromFamilyHandler : IRequestHandler<RemoveUserFromFamilyCommand>
    {
        private readonly IConnectionManager _con;

        public RemoveUserFromFamilyHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(RemoveUserFromFamilyCommand request, CancellationToken cancellationToken)
        {
            await _con.GetConnection()
                .ExecuteAsync(@"
                    delete from user_families
                    where user_id = @userId",
                    new {userId = request.UserId});

            return new Unit();
        }
    }
}
