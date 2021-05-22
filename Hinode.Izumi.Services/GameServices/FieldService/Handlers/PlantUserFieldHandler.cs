using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.FieldService.Commands;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FieldService.Handlers
{
    public class PlantUserFieldHandler : IRequestHandler<PlantUserFieldCommand>
    {
        private readonly IConnectionManager _con;
        private readonly IMediator _mediator;

        public PlantUserFieldHandler(IConnectionManager con, IMediator mediator)
        {
            _con = con;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(PlantUserFieldCommand request, CancellationToken cancellationToken)
        {
            var (userId, fieldId, seedId) = request;
            var weather = (Weather) await _mediator.Send(
                new GetPropertyValueQuery(Property.WeatherToday), cancellationToken);

            await _con.GetConnection()
                .ExecuteAsync(@"
                    update user_fields
                    set seed_id = @seedId,
                        state = @state,
                        updated_at = now()
                    where user_id = @userId
                      and field_id = @fieldId",
                    new
                    {
                        userId, fieldId, seedId,
                        state = weather == Weather.Clear
                            ? FieldState.Planted
                            : FieldState.Watered
                    });

            return new Unit();
        }
    }
}
