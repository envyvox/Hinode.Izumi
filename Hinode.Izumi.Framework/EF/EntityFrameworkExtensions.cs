using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Hinode.Izumi.Framework.EF
{
    public static class EntityFrameworkExtensions
    {
        public static void MigrateDb(this IServiceProvider app)
        {
            using var serviceScope = app.GetService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetService<DbContext>();
            context.Database.Migrate();
        }
    }
}
