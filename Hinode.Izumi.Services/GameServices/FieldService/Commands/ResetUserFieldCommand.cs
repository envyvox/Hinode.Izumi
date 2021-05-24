using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FieldService.Commands
{
    public record ResetUserFieldCommand(long UserId, long FieldId) : IRequest;

    public class ResetUserFieldHandler : IRequestHandler<ResetUserFieldCommand>
    {
        private readonly IConnectionManager _con;

        public ResetUserFieldHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(ResetUserFieldCommand request, CancellationToken cancellationToken)
        {
            var (userId, fieldId) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    update user_fields
                    set state = default,
                        seed_id = default,
                        progress = default,
                        re_growth = default,
                        updated_at = now()
                    where user_id = @userId
                      and field_id = @fieldId",
                    new {userId, fieldId});

            return new Unit();
        }
    }
}
