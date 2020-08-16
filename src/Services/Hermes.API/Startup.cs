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
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Nubles.Core;
using Nubles.Infrastructure;
using System;
using System.IO;
using System.Reflection;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]

namespace Hermes.API
{
    public class Startup
    {
        public static readonly ILoggerFactory HermesLoggerFactory
            = LoggerFactory.Create(builder => { builder.AddConsole(); });

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
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

            services.AddNublesCore()
                    .AddNublesInfrastructure(Configuration, HermesLoggerFactory);

            services.AddSwaggerGenWithOptions();

            services.AddJwtAuthentication();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                         .RequireAuthorization(AuthConstants.Policy.ApiScope);
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
                    .AddJwtBearer(configureOptions =>
                    {
                        // base address for IdentityServer
                        configureOptions.Authority = AuthConstants.Identity.AuthorityAddress;

                        configureOptions.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidTypes = new[]
                            {
                                // IdentityServer emits a typ header by default, recommended extra check
                                "at+jwt"
                            },
                            // Using scope only authentication
                            ValidateAudience = false
                        };
                    });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthConstants.Policy.ApiScope, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", AuthConstants.Identity.ScopeName);
                });
            });

            return services;
        }
    }
}