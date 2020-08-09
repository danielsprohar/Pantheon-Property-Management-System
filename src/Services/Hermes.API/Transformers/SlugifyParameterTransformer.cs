using Microsoft.AspNetCore.Routing;
using System;
using System.Text.RegularExpressions;

namespace Hermes.API.Transformers
{
    public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        public string TransformOutbound(object value)
        {
            if (value == null) { return null; }

            // https://docs.microsoft.com/en-us/aspnet/core/razor-pages/razor-pages-conventions?view=aspnetcore-3.1#use-a-parameter-transformer-to-customize-page-routes
            return Regex.Replace(input: value.ToString(),
                                 pattern: "([a-z])([A-Z])",
                                 replacement: "$1-$2",
                                 options: RegexOptions.CultureInvariant,
                                 matchTimeout: TimeSpan.FromMilliseconds(100)).ToLowerInvariant();
        }
    }
}