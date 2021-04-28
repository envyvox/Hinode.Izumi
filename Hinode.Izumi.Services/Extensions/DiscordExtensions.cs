using Hinode.Izumi.Services.DiscordServices.DiscordClientService;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Hinode.Izumi.Services.Extensions
{
    public static class DiscordExtensions
    {
        public static IApplicationBuilder StartDiscord(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var service = serviceScope.ServiceProvider.GetService<IDiscordClientService>();
            service.Start().Wait();

            return app;
        }
    }
}
