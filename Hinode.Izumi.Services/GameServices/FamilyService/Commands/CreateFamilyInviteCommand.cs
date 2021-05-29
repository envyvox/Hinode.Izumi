using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Commands
{
    public record CreateFamilyInviteCommand(long FamilyId, long UserId) : IRequest;

    public class CreateFamilyInviteHandler : IRequestHandler<CreateFamilyInviteCommand>
    {
        private readonly IConnectionManager _con;

        public CreateFamilyInviteHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(CreateFamilyInviteCommand request, CancellationToken cancellationToken)
        {
            var (familyId, userId) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into family_invites(family_id, user_id)
                    values (@familyId, @userId)
                    on conflict (family_id, user_id) do nothing",
                    new {familyId, userId});

            return new Unit();
        }
    }
}
