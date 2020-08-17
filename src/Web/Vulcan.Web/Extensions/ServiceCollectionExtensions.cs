using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.DependencyInjection;
using Pantheon.Identity.Constants;
using System.IdentityModel.Tokens.Jwt;

namespace Vulcan.Web.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterAuthenticaionMiddleware(
            this IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = IdentityServerConstants.ProtocolTypes.OpenIdConnect;
            })
                .AddCookie("Cookies") // add the handler that can process cookies
                .AddOpenIdConnect(IdentityServerConstants.ProtocolTypes.OpenIdConnect, options =>
                {
                    // The address of Identity Server
                    options.Authority = "https://localhost:5001";

                    options.ClientId = PantheonIdentityConstants.Clients.Vulcan;

                    // TODO: Store the secret in a secure store
                    options.ClientSecret = "MyNotSoSecretSecret_0987".Sha256();

                    // https://identityserver4.readthedocs.io/en/latest/topics/grant_types.html#interactive-clients
                    options.ResponseType = "code";

                    options.SaveTokens = true;

                    // so we can access the API resources
                    options.Scope.Add(PantheonIdentityConstants.ApiScopes.Hermes);
                });

            return services;
        }
    }
}