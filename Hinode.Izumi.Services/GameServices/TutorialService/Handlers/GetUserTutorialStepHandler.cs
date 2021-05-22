using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.TutorialService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.TutorialService.Handlers
{
    public class GetUserTutorialStepHandler : IRequestHandler<GetUserTutorialStepQuery, TutorialStep>
    {
        private readonly IConnectionManager _con;

        public GetUserTutorialStepHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<TutorialStep> Handle(GetUserTutorialStepQuery request, CancellationToken cancellationToken)
        {
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<TutorialStep>(@"
                    select step from user_trainings
                    where user_id = @userId",
                    new {userId = request.UserId});
        }
    }
}
