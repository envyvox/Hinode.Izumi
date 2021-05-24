using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.TutorialService.Commands
{
    public record UpdateUserTutorialStepCommand(long UserId, TutorialStep Step) : IRequest;

    public class UpdateUserTutorialStepHandler : IRequestHandler<UpdateUserTutorialStepCommand>
    {
        private readonly IConnectionManager _con;

        public UpdateUserTutorialStepHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(UpdateUserTutorialStepCommand request, CancellationToken cancellationToken)
        {
            var (userId, step) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_trainings(user_id, step)
                    values (@userId, @step)
                    on conflict (user_id) do update
                        set step = @step,
                            updated_at = now()",
                    new {userId, step});

            return new Unit();
        }
    }
}
