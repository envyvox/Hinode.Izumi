using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FieldService.Commands
{
    public record UpdateAllFieldsStateCommand(FieldState State) : IRequest;

    public class UpdateAllFieldsStateHandler : IRequestHandler<UpdateAllFieldsStateCommand>
    {
        private readonly IConnectionManager _con;

        public UpdateAllFieldsStateHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(UpdateAllFieldsStateCommand request, CancellationToken cancellationToken)
        {
            await _con.GetConnection()
                .ExecuteAsync(@"
                    update user_fields
                    set state = @state,
                        updated_at = now()
                    where state != @empty
                      and state != @completed",
                    new {state = request.State, empty = FieldState.Empty, completed = FieldState.Completed});

            return new Unit();
        }
    }
}
