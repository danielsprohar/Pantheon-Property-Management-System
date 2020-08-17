namespace Pantheon.IdentityServer.Constants
{
    public class PantheonIdentityConstants
    {
        /// <summary>
        /// The url to Identity Server
        /// </summary>
        public const string AuthorityAddress = "https://localhost:6001";

        public class ApiScopes
        {
            /// <summary>
            /// The client id for the Hermes API that is used by Identity Server
            /// </summary>
            public const string Hermes = "hermes.api";
        }

        public class Clients
        {
            /// <summary>
            /// The client id for the Vulcan webapp that is used by Identity Server
            /// </summary>
            public const string Vulcan = "vulcan.webapp";
        }
    }
}