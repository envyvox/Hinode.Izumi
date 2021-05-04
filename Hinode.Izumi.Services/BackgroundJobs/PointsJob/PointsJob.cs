using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;

namespace Hinode.Izumi.Services.BackgroundJobs.PointsJob
{
    [InjectableService]
    public class PointsJob : IPointsJob
    {
        private readonly IConnectionManager _con;

        public PointsJob(IConnectionManager con)
        {
            _con = con;
        }

        public async Task ResetAdventurePoints() =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    update users
                    set points = 0,
                        updated_at = now()
                    where points > 0");
    }
}
