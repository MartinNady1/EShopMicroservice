using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Infrastructure
{
    public static class DependencyInjection
    {
            public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
            {
                // Register infrastructure services here (e.g., database context, repositories, etc.)
                // Example:
                // services.AddDbContext<OrderingDbContext>(options =>
                //     options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    
                return services;
        }
    }
}
