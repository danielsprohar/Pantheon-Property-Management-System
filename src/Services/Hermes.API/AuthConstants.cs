namespace Hermes.API
{
    public class AuthConstants
    {
        public class Identity
        {
            /// <summary>
            /// The base address for IdentityServer
            /// </summary>
            public const string AuthorityAddress = "https://localhost:5001";

            /// <summary>
            /// The scope name for Hermes API
            /// </summary>
            public const string ScopeName = "hermes.api";
        }

        public class Policy
        {
            public const string ApiScope = "ApiScope";
        }
    }
}