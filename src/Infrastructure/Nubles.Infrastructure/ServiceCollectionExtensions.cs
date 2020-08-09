using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Nubles.Infrastructure.Data;
using Microsoft.Extensions.Configuration;

namespace Nubles.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNublesInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<PantheonDbContext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    providerOptions => providerOptions.MigrationsAssembly(
                        typeof(PantheonDbContext).Assembly.FullName));
            });

            return services;
        }
    }
}
