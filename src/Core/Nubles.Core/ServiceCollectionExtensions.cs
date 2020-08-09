using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Nubles.Core.Application.Mappings;

namespace Nubles.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNublesCore(this IServiceCollection services)
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