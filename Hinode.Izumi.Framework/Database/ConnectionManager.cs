using System.Data;

namespace Hinode.Izumi.Framework.Database
{
    public abstract class ConnectionManagerBase : IConnectionManager
    {
        public IDbConnection GetConnection()
        {
            return _connection ??= CreateDbConnection();
        }

        private IDbConnection _connection;
        protected abstract IDbConnection CreateDbConnection();
    }
}
