using System;
using System.Collections.Generic;
using System.Web;

namespace Pantheon.Core.Application.Extensions
{
    public static class UriStringExtensions
    {
        public static string AppendRouteValues(this string uri, IEnumerable<KeyValuePair<string, object>> routeValues)
        {
            if (string.IsNullOrEmpty(uri))
            {
                throw new ArgumentException($"'{nameof(uri)}' cannot be null or empty", nameof(uri));
            }

            if (routeValues is null)
            {
                throw new ArgumentNullException(nameof(routeValues));
            }

            var uriBuilder = new UriBuilder(uri);

            var currentRouteValues = HttpUtility.ParseQueryString(uriBuilder.Query);

            foreach (var value in routeValues)
            {
                if (value.Value != null)
                {
                    currentRouteValues.Add(value.Key, value.Value?.ToString());
                }
            }

            uriBuilder.Query = currentRouteValues.ToString();

            return uriBuilder.Uri.AbsoluteUri;
        }
    }
}