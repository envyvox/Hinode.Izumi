using System.Threading.Tasks;
using Dapper;
using Hangfire;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;

namespace Hinode.Izumi.Services.BackgroundJobs.EnergyJob
{
    [InjectableService]
    public class EnergyJob : IEnergyJob
    {
        private readonly IConnectionManager _con;

        public EnergyJob(IConnectionManager con)
        {
            _con = con;
        }

        public async Task HourlyRecovery() =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    update users
                    set energy = energy + 1,
                        updated_at = now()
                    where energy < 100");
    }
}
