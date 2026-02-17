using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Data;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Data.Interceptors;


namespace Ordering.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISaveChangesInterceptor , AuditEntityInterceptor>();
            services.AddScoped<ISaveChangesInterceptor , DispatchDomainEventInterceptor>();
            

            var connectionString = configuration.GetConnectionString("Database");
            services.AddDbContext<ApplicationDbContexct>((sp, options) => {
                options.UseNpgsql(connectionString); 
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
               
                });
            services.AddScoped<IApplicationDbContext,ApplicationDbContexct>();

            return services;
        }
    }   
}
