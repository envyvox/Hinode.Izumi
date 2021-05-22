using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.FamilyService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Handlers
{
    public class RemoveCurrencyFromFamilyHandler : IRequestHandler<RemoveCurrencyFromFamilyCommand>
    {
        private readonly IConnectionManager _con;

        public RemoveCurrencyFromFamilyHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(RemoveCurrencyFromFamilyCommand request, CancellationToken cancellationToken)
        {
            var (familyId, currency, amount) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    update family_currencies
                    set amount = amount - @amount,
                        updated_at = now()
                    where family_id = @familyId",
                    new {familyId, currency, amount});

            return new Unit();
        }
    }
}
