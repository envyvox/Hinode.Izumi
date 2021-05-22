using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.FamilyEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.FamilyService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Handlers
{
    public class CreateFamilyHandler : IRequestHandler<CreateFamilyCommand>
    {
        private readonly IConnectionManager _con;

        public CreateFamilyHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(CreateFamilyCommand request, CancellationToken cancellationToken)
        {
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into families(status, name, description)
                    values (@status, @name, null)
                    on conflict (name) do nothing",
                    new {name = request.Name, status = FamilyStatus.Registration});

            return new Unit();
        }
    }
}
