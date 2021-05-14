using System;
using System.Collections;
using System.Reflection;
using Autofac;
using Dapper;
using Discord.Commands;
using Hangfire;
using Hangfire.PostgreSql;
using Hinode.Izumi.Data;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Framework.Database.DapperHandlers;
using Hinode.Izumi.Framework.EF;
using Hinode.Izumi.Framework.Hangfire;
using Hinode.Izumi.Framework.Web;
using Hinode.Izumi.Services.DiscordServices.DiscordClientService;
using Hinode.Izumi.Services.DiscordServices.DiscordClientService.Impl;
using Hinode.Izumi.Services.DiscordServices.DiscordClientService.Options;
using Hinode.Izumi.Services.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hinode.Izumi
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/dist"; });
            services.AddControllers(x => x.Conventions.Add(new ApiRouteConvention()))
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddJsonOptions(x => { x.JsonSerializerOptions.Converters.Add(new TimeSpanSerializer()); });
            services.AddOpenApiDocument(x => x.DocumentName = "api");
            services
                .AddHealthChecks()
                .AddNpgSql(_config.GetConnectionString("main"))
                .AddDbContextCheck<AppDbContext>();
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            SqlMapper.AddTypeHandler(new TimeSpanHandler());
            services.Configure<ConnectionOptions>(x => x.ConnectionString = _config.GetConnectionString("main"));
            services.Configure<DiscordOptions>(x => _config.GetSection("Discord").Bind(x));
            services.AddSingleton<CommandService>();
            services.AddSingleton<IDiscordClientService, DiscordClientService>();
            services.AddMemoryCache();
            services.AddHttpClient();
            services.AddControllers().AddNewtonsoftJson();
            // add timezone info
            services.AddSingleton(x =>
                TimeZoneInfo.FindSystemTimeZoneById(_config.GetValue<string>("CronTimezoneId")));
            // add db context
            services.AddDbContextPool<DbContext, AppDbContext>(o =>
            {
                o.UseNpgsql(_config.GetConnectionString("main"),
                    s => { s.MigrationsAssembly(typeof(AppDbContext).Assembly.GetName().Name); });
            });
            // add hangfire
            services.AddHangfire(config =>
            {
                // отключаем повторный запуск джобы по-умолчанию если произошла ошибка
                GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute {Attempts = 0});
                // добавляем соединение с базой
                config.UsePostgreSqlStorage(_config.GetConnectionString("main"));
            });
            // add swagger
            services.AddSwaggerDocument();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.ApplicationServices.MigrateDb();
            app.UseForwardedHeaders(
                new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                                       ForwardedHeaders.XForwardedProto
                });
            app.Use(async (context, next) =>
            {
                if (_config.GetValue<string>("AccessType")?.ToLower() != "whitelist")
                {
                    await next();
                    return;
                }
                var ips = (_config.GetValue<string>("AllowedIps") ?? "").Split(';');
                var remoteIp = context.Request.Headers["X-Real-IP"].ToString();
                if (!((IList) ips).Contains(remoteIp))
                {
                    await context.Response.WriteAsync($@"
                        <!doctype html>
                        <html>
                            <body>
                                <div>
                                    <h2>Oops, seems that you are not authorized to access this page</h2>
                                <div>
                                <footer>
                                    <small>Your IP logged: {remoteIp}</small>
                                </footer>
                            </body>
                        </html>");
                    return;
                }

                await next();
            });

            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] {new AllowAllAuthorizationFilter()}
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseOpenApi();
            app.UseRouting();
            app.UseSwaggerUi3();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/healthz");
            });
            app.UseSpaStaticFiles();
            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });

            app.StartDiscord();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            var assembly = typeof(IDiscordClientService).Assembly;
            builder.RegisterAssemblyTypes(assembly)
                .Where(x => x.IsDefined(typeof(InjectableServiceAttribute), false) &&
                            x.GetCustomAttribute<InjectableServiceAttribute>().IsSingletone)
                .As(x => x.GetInterfaces()[0])
                .SingleInstance();

            builder.RegisterAssemblyTypes(assembly)
                .Where(x => x.IsDefined(typeof(InjectableServiceAttribute), false) &&
                            !x.GetCustomAttribute<InjectableServiceAttribute>().IsSingletone)
                .As(x => x.GetInterfaces()[0])
                .InstancePerLifetimeScope();
        }
    }
}
