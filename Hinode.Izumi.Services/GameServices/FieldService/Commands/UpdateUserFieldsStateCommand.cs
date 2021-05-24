using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FieldService.Commands
{
    public record UpdateUserFieldsStateCommand(long UserId, FieldState State) : IRequest;

    public class UpdateUserFieldsStateHandler : IRequestHandler<UpdateUserFieldsStateCommand>
    {
        private readonly IConnectionManager _con;

        public UpdateUserFieldsStateHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(UpdateUserFieldsStateCommand request, CancellationToken cancellationToken)
        {
            var (userId, state) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    update user_fields
                    set state = @state,
                        updated_at = now()
                    where user_id = @userId
                      and state = @planted",
                    new {userId, state, planted = FieldState.Planted});

            return new Unit();
        }
    }
}
