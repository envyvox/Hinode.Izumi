using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.FamilyEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.FamilyService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Handlers
{
    public class AddUserToFamilyByFamilyIdHandler : IRequestHandler<AddUserToFamilyByFamilyIdCommand>
    {
        private readonly IConnectionManager _con;
        private readonly IMediator _mediator;

        public AddUserToFamilyByFamilyIdHandler(IConnectionManager con, IMediator mediator)
        {
            _con = con;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(AddUserToFamilyByFamilyIdCommand request, CancellationToken cancellationToken)
        {
            var (userId, familyId) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_families(user_id, family_id, status)
                    values (@userId, @familyId, @status)
                    on conflict (user_id) do nothing",
                    new {userId, familyId, status = UserInFamilyStatus.Default});

            await _mediator.Send(new CheckFamilyRegistrationCompleteCommand(familyId), cancellationToken);

            return new Unit();
        }
    }
}
