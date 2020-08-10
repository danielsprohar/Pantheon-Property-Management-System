﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nubles.Infrastructure.Data;

namespace Nubles.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNublesInfrastructure(
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