using Microsoft.AspNetCore.Mvc;
using Pantheon.Core.Application.Parameters;
using Pantheon.Core.Application.Wrappers.Generics;
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

        public PaginatedApiResponse<T> GenerateLinks(string routeName, QueryParameters parameters)
        {
            var originalPageIndex = parameters.PageIndex;

            if (_response.HasPrevious())
            {
                parameters.PageIndex--;
                // TODO: Validate Host header before deployment
                // Or, created a string constant of the application's base address
                _response.PreviousPage = _urlHelper.Link(
                    routeName,
                    parameters.GetRouteValues());

                parameters.PageIndex = originalPageIndex;
            }
            if (_response.HasNext())
            {
                parameters.PageIndex++;

                _response.NextPage = _urlHelper.Link(
                    routeName,
                    parameters.GetRouteValues());

                parameters.PageIndex = originalPageIndex;
            }

            parameters.PageIndex = 0;

            _response.FirstPage = _urlHelper.Link(
                    routeName,
                    parameters.GetRouteValues());

            parameters.PageIndex = _response.TotalPages;

            _response.LastPage = _urlHelper.Link(
                    routeName,
                    parameters.GetRouteValues());

            parameters.PageIndex = originalPageIndex;

            return _response;
        }
    }
}