using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Pantheon.Core.Application.Mappings;

namespace Pantheon.Core.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPantheonCoreLayer(this IServiceCollection services)
        {
            var configuration = new MapperConfiguration(config =>
            {
                config.AddProfile<NublesProfile>();
            });

            configuration.AssertConfigurationIsValid();

            services.AddAutoMapper(typeof(NublesProfile).Assembly);

            return services;
        }
    }
}