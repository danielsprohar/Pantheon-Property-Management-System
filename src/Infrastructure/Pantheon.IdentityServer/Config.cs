using IdentityServer4;
using IdentityServer4.Models;
using Pantheon.IdentityServer.Constants;
using System.Collections.Generic;

namespace Pantheon.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                // https://auth0.com/docs/scopes/openid-connect-scopes
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                // https://auth0.com/docs/scopes/api-scopes
                // https://identityserver4.readthedocs.io/en/latest/topics/resources.html#scopes
                new ApiScope(PantheonIdentityConstants.ApiScopes.Hermes, "Hermes API"),
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = PantheonIdentityConstants.Clients.Vulcan,
                    ClientName = "Vulcan Webapp",
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowOfflineAccess = true,
                    ClientSecrets =
                    {
                        new Secret("MyNotSoSecretSecret_0987".Sha256())
                    },
                    RedirectUris =
                    {
                        "https://localhost:5002/signin-oidc"
                    },
                    PostLogoutRedirectUris =
                    {
                        "https://localhost:5002/"
                    },
                    FrontChannelLogoutUri = "https://localhost:5002/signout-oidc",
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        PantheonIdentityConstants.ApiScopes.Hermes
                    },
                }
            };
    }
}