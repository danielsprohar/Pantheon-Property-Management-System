using IdentityServer4;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pantheon.Identity.Constants;
using System.IdentityModel.Tokens.Jwt;
using Vulcan.Web.Options;
using Vulcan.Web.Services;

namespace Vulcan.Web
{
    public class Startup
    {
        private const string CookieScheme = "Pantheon.Cookie";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieScheme;
                options.DefaultChallengeScheme = IdentityServerConstants.ProtocolTypes.OpenIdConnect;
            })
                .AddCookie(CookieScheme) // add the handler that can process cookies
                .AddOpenIdConnect(IdentityServerConstants.ProtocolTypes.OpenIdConnect, options =>
                {
                    // The address of Identity Server
                    options.Authority = PantheonIdentityConstants.AuthorityAddress;

                    options.ClientId = PantheonIdentityConstants.Clients.Vulcan;

                    // TODO: Store the secret in a secure store
                    options.ClientSecret = "MyNotSoSecretSecret_0987";

                    // https://identityserver4.readthedocs.io/en/latest/topics/grant_types.html#interactive-clients
                    options.ResponseType = "code";

                    options.SaveTokens = true;

                    options.GetClaimsFromUserInfoEndpoint = true;

                    // so we can access the API resources
                    options.Scope.Add(PantheonIdentityConstants.ApiScopes.Hermes);
                });

            //services.AddRazorPages(options =>
            //{
            //    //options.Conventions.AuthorizeAreaFolder("Identity")
            //});

            services.AddOptions();

            services.Configure<HermesApiOptions>(Configuration.GetSection(HermesApiOptions.HermesApi));

            services.AddHttpContextAccessor();

            services.AddHttpClient<IParkingSpaceService, ParkingSpaceService>();

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
                endpoints.MapRazorPages().RequireAuthorization();
            });
        }
    }
}