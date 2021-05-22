using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.FamilyEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.FamilyService.Commands;
using Hinode.Izumi.Services.GameServices.FamilyService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Handlers
{
    public class AddUserToFamilyByFamilyNameHandler : IRequestHandler<AddUserToFamilyByFamilyNameCommand>
    {
        private readonly IConnectionManager _con;
        private readonly IMediator _mediator;

        public AddUserToFamilyByFamilyNameHandler(IConnectionManager con, IMediator mediator)
        {
            _con = con;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(AddUserToFamilyByFamilyNameCommand request, CancellationToken cancellationToken)
        {
            var (userId, familyName) = request;
            var family = await _mediator.Send(new GetFamilyByNameQuery(familyName), cancellationToken);

            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_families(user_id, family_id, status)
                    values (@userId, @familyId, @status)
                    on conflict (user_id) do nothing",
                    new {userId, familyId = family.Id, status = UserInFamilyStatus.Head});

            return new Unit();
        }
    }
}
