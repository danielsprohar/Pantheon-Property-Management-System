using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pantheon.Identity.Data;
using Pantheon.Identity.Models;

namespace Pantheon.Identity
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPantheonIdentityInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration,
            ILoggerFactory loggerFactory = null)
        {
            services.AddDbContext<PantheonIdentityDbContext>(optionsBuilder =>
            {
                if (loggerFactory != null)
                {
                    optionsBuilder.UseLoggerFactory(loggerFactory);
                }

                optionsBuilder.UseSqlServer(
                    configuration.GetConnectionString("Identity"),
                    providerOptions => providerOptions.MigrationsAssembly(
                        typeof(PantheonIdentityDbContext).Assembly.FullName));
            });

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<PantheonIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
            });

            return services;
        }
    }
}