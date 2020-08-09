using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nubles.Infrastructure.Data;

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