using Carter;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Ordering.API
{
    public static class DependencyInjection
    {
            public static IServiceCollection AddApiServices(this IServiceCollection services , IConfiguration Configuration)
            {
                services.AddCarter();
            services.AddHealthChecks().AddNpgSql(Configuration.GetConnectionString("Database")!);
                return services;
            }
        public static WebApplication UseApiServices(this WebApplication app)
        {
            app.MapCarter();
            app.UseHealthChecks("/api/health" , new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            return app;
        }
    }
}