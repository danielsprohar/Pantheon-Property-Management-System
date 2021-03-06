using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Pantheon.Core.Application.Extensions;
using Pantheon.Core.Application.Services;
using Pantheon.Identity.Constants;
using System.IdentityModel.Tokens.Jwt;
using Vulcan.Web.Constants;
using Vulcan.Web.Options;
using Vulcan.Web.Services;

namespace Vulcan.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddPantheonCoreLayer();
            
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = AppConstants.CookieScheme;
                options.DefaultChallengeScheme = AppConstants.DefaultChallangeScheme;
            })
                .AddCookie(AppConstants.CookieScheme) // add the handler that can process cookies
                .AddOpenIdConnect(AppConstants.DefaultChallangeScheme, options =>
                {
                    // The address of Identity Server
                    options.Authority = PantheonIdentityConstants.AuthorityAddress;

                    options.ClientId = PantheonIdentityConstants.Clients.Vulcan;

                    options.GetClaimsFromUserInfoEndpoint = true;

                    // TODO: Store the secret in a secure store
                    options.ClientSecret = "MyNotSoSecretSecret_0987";

                    // https://identityserver4.readthedocs.io/en/latest/topics/grant_types.html#interactive-clients
                    options.ResponseType = "code";

                    options.SaveTokens = true;

                    // so we can access the API resources
                    options.Scope.Add(PantheonIdentityConstants.ApiScopes.Hermes);
                });

            services.AddOptions();

            services.Configure<HermesApiOptions>(Configuration.GetSection(HermesApiOptions.HermesApi));

            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddHttpContextAccessor();
            services.AddHttpClient<IParkingSpaceService, ParkingSpaceService>();
            services.AddHttpClient<IParkingSpaceTypeService, ParkingSpaceTypeService>();

            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                         //.RequireAuthorization();
            });
        }
    }
}