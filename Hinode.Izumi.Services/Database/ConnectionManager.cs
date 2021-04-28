using System.Data;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Hinode.Izumi.Services.Database
{
    [InjectableService]
    public class ConnectionManager : ConnectionManagerBase
    {
        private readonly ConnectionOptions _options;

        public ConnectionManager(IOptions<ConnectionOptions> options)
        {
            _options = options.Value;
        }

        protected override IDbConnection CreateDbConnection() => new NpgsqlConnection(_options.ConnectionString);
    }
}
