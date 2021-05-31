using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.BackgroundJobs.EnergyJob
{
    [InjectableService]
    public class EnergyJob : IEnergyJob
    {
        private readonly IConnectionManager _con;
        private readonly IMediator _mediator;

        public EnergyJob(IConnectionManager con, IMediator mediator)
        {
            _con = con;
            _mediator = mediator;
        }

        public async Task HourlyRecovery()
        {
            var energyNonPremium = await _mediator.Send(new GetPropertyValueQuery(
                Property.EnergyRecoveryPerHourNonPremium));
            var energyPremium = await _mediator.Send(new GetPropertyValueQuery(
                Property.EnergyRecoveryPerHourPremium));

            await _con.GetConnection()
                .ExecuteAsync(@"
                    update users
                    set energy = (
                        case when energy + @energyNonPremium <= 100 then energy + @energyNonPremium
                             else 100
                        end),
                        updated_at = now()
                    where premium = false;

                    update users
                    set energy = (
                        case when energy + @energyPremium <= 100 then energy + @energyPremium
                             else 100
                        end),
                        updated_at = now()
                    where premium = true;",
                    new {energyNonPremium, energyPremium});
        }
    }
}
