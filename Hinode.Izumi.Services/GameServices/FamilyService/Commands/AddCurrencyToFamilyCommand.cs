using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Commands
{
    public record AddCurrencyToFamilyCommand(long FamilyId, Currency Currency, long Amount) : IRequest;

    public class AddCurrencyToFamilyHandler : IRequestHandler<AddCurrencyToFamilyCommand>
    {
        private readonly IConnectionManager _con;

        public AddCurrencyToFamilyHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(AddCurrencyToFamilyCommand request, CancellationToken cancellationToken)
        {
            var (familyId, currency, amount) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into family_currencies as fc (family_id, currency, amount)
                    values (@familyId, @currency, @amount)
                    on conflict (family_id, currency) do update
                        set amount = fc.amount + @amount,
                            updated_at = now()",
                    new {familyId, currency, amount});

            return new Unit();
        }
    }
}
