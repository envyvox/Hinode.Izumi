using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.FieldService.Records;
using Hinode.Izumi.Services.GameServices.SeedService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FieldService.Commands
{
    public record MoveAllFieldsProgressCommand : IRequest;

    public class MoveAllFieldsProgressHandler : IRequestHandler<MoveAllFieldsProgressCommand>
    {
        private readonly IConnectionManager _con;
        private readonly IMediator _mediator;

        public MoveAllFieldsProgressHandler(IConnectionManager con, IMediator mediator)
        {
            _con = con;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(MoveAllFieldsProgressCommand request, CancellationToken cancellationToken)
        {
            await _con.GetConnection()
                .ExecuteAsync(@"
                    update user_fields
                    set progress = progress + 1,
                        updated_at = now()
                    where state = @watered",
                    new {watered = FieldState.Watered});
            await CheckFieldComplete();

            return new Unit();
        }

        private async Task CheckFieldComplete()
        {
            var fields = await _con.GetConnection()
                .QueryAsync<UserFieldRecord>(@"
                    select * from user_fields
                    where state != @empty
                      and state != @completed",
                    new {empty = FieldState.Empty, completed = FieldState.Completed});

            var completedFields = new List<long>();
            foreach (var field in fields)
            {
                var seed = await _mediator.Send(new GetSeedQuery(field.SeedId));

                if (field.ReGrowth is false && field.Progress >= seed.Growth) completedFields.Add(field.Id);
                if (field.ReGrowth is true && field.Progress >= seed.ReGrowth) completedFields.Add(field.Id);
            }

            await _con.GetConnection()
                .ExecuteAsync(@"
                    update user_fields
                    set state = @completed,
                        updated_at = now()
                    from (
                        select unnest(array[@ids]) as cId
                        ) as new
                    where id = new.cId",
                    new {completed = FieldState.Completed, ids = completedFields.ToArray()});
        }
    }
}
