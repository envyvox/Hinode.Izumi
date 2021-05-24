using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Commands
{
    public record RemoveCurrencyFromFamilyCommand(long FamilyId, Currency Currency, long Amount) : IRequest;

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
