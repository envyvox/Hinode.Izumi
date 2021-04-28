using System.Data;

namespace Hinode.Izumi.Framework.Database
{
    public interface IConnectionManager
    {
        IDbConnection GetConnection();
    }
}
