using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FieldService.Commands
{
    public record StartReGrowthOnUserFieldCommand(long UserId, long FieldId) : IRequest;

    public class StartReGrowthOnUserFieldHandler : IRequestHandler<StartReGrowthOnUserFieldCommand>
    {
        private readonly IConnectionManager _con;
        private readonly IMediator _mediator;

        public StartReGrowthOnUserFieldHandler(IConnectionManager con, IMediator mediator)
        {
            _con = con;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(StartReGrowthOnUserFieldCommand request, CancellationToken cancellationToken)
        {
            var (userId, fieldId) = request;
            var weather = (Weather) await _mediator.Send(
                new GetPropertyValueQuery(Property.WeatherToday), cancellationToken);

            await _con.GetConnection()
                .ExecuteAsync(@"
                    update user_fields
                    set state = @state,
                        progress = 0,
                        re_growth = true,
                        updated_at = now()
                    where user_id = @userId
                      and field_id = @fieldId",
                    new
                    {
                        userId, fieldId,
                        state = weather == Weather.Clear
                            ? FieldState.Planted
                            : FieldState.Watered
                    });

            return new Unit();
        }
    }
}
