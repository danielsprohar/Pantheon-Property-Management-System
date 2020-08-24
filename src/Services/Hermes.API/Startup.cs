using Hermes.API.Constants;
using Hermes.API.Transformers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Pantheon.Core.Application.Extensions;
using Pantheon.Identity;
using Pantheon.Identity.Constants;
using Pantheon.Infrastructure;
using System;
using System.IO;
using System.Reflection;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]

namespace Hermes.API
{
    public class Startup
    {
        public static readonly ILoggerFactory HermesLoggerFactory
            = LoggerFactory.Create(builder => builder.AddConsole());

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddPantheonIdentityInfrastructure(Configuration);

            services.AddControllers(config =>
            {
                var pageRouteTransformer = new RouteTokenTransformerConvention(new SlugifyParameterTransformer());
                config.Conventions.Add(pageRouteTransformer);
            })
                .AddNewtonsoftJson(); //https://docs.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-3.1#json-patch-addnewtonsoftjson-and-systemtextjson

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(majorVersion: 1, minorVersion: 0);
            });

            services.AddPantheonCoreLayer()
                    .AddPantheonInfrastructure(Configuration, HermesLoggerFactory);

            services.AddSwaggerGenWithOptions();

            services.AddJwtAuthentication();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region Swagger

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hermes' API ");
            });

            #endregion Swagger

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                    .RequireAuthorization(HermesConstants.AuthorizationPolicy.ApiScope);
            });
        }
    }

    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSwaggerGenWithOptions(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Hermes' API v1",
                    Description = "A RESTful service for Pantheon Management System",
                    Contact = new OpenApiContact
                    {
                        Name = "Daniel Sprohar",
                        Email = string.Empty,
                        Url = new Uri("https://github.com/danielsprohar"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under MIT",
                        Url = new Uri("https://github.com/danielsprohar/Pantheon-Management-System/blob/main/LICENSE"),
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                    {
                        // base address for IdentityServer
                        options.Authority = PantheonIdentityConstants.AuthorityAddress;

                        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                        {
                            // Using "scope only" authentication
                            ValidateAudience = false,
                            SaveSigninToken = true
                            //ValidTypes = new[]
                            //{
                            //    // IdentityServer emits a typ header by default, recommended extra check
                            //    "at+jwt"
                            //}
                        };

                        //options.Events = new JwtBearerEvents
                        //{
                        //    OnChallenge = c =>
                        //    {
                                
                        //    }
                        //};
                    });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(HermesConstants.AuthorizationPolicy.ApiScope, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", PantheonIdentityConstants.ApiScopes.Hermes);
                });
            });

            return services;
        }
    }
}