using Hangfire.Dashboard;

namespace Hinode.Izumi.Framework.Hangfire
{
    public class AllowAllAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context) => true;
    }
}
