using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.FieldService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FieldService.Handlers
{
    public class ResetAllFieldsHandler : IRequestHandler<ResetAllFieldsCommand>
    {
        private readonly IConnectionManager _con;

        public ResetAllFieldsHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(ResetAllFieldsCommand request, CancellationToken cancellationToken)
        {
            await _con.GetConnection()
                .ExecuteAsync(@"
                    update user_fields
                    set state = default,
                        seed_id = default,
                        progress = default,
                        re_growth = default,
                        updated_at = now()");

            return new Unit();
        }
    }
}
