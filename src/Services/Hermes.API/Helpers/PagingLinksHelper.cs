using Microsoft.AspNetCore.Mvc;
using Nubles.Core.Application.Parameters;
using Nubles.Core.Application.Wrappers.Generics;
using System;

namespace Hermes.API.Helpers
{
    public class PagingLinksHelper<T> where T : class
    {
        private readonly PaginatedApiResponse<T> _response;
        private readonly IUrlHelper _urlHelper;

        public PagingLinksHelper(PaginatedApiResponse<T> response, IUrlHelper urlHelper)
        {
            _response = response ?? throw new ArgumentNullException(nameof(response));
            _urlHelper = urlHelper ?? throw new ArgumentNullException(nameof(urlHelper));
        }

        public PaginatedApiResponse<T> GenerateLinks(string routeName, QueryParameters queryParameters)
        {
            if (_response.HasPrevious())
            {
                _response.PreviousPage = _urlHelper.Link(
                    routeName,
                    new QueryParameters(queryParameters.PageIndex - 1, queryParameters.PageSize));
            }
            if (_response.HasNext())
            {
                _response.NextPage = _urlHelper.Link(
                    routeName,
                    new QueryParameters(queryParameters.PageIndex + 1, queryParameters.PageSize));
            }

            _response.FirstPage = _urlHelper.Link(
                    routeName,
                    new QueryParameters(0, queryParameters.PageSize));

            _response.LastPage = _urlHelper.Link(
                    routeName,
                    new QueryParameters(_response.TotalPages - 1, queryParameters.PageSize));

            return _response;
        }
    }
}