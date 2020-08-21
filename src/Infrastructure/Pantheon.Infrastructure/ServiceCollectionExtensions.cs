using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pantheon.Infrastructure.Data;

namespace Pantheon.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPantheonInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration,
            ILoggerFactory loggerFactory = null)
        {
            services.AddDbContext<PantheonDbContext>(optionsBuilder =>
            {
                if (loggerFactory != null)
                {
                    optionsBuilder.UseLoggerFactory(loggerFactory);
                }

                optionsBuilder.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    providerOptions => providerOptions.MigrationsAssembly(
                        typeof(PantheonDbContext).Assembly.FullName));
            });

            return services;
        }
    }
}