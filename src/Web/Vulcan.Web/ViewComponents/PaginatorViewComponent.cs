using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Pantheon.Core.Application.Parameters;
using System;
using Vulcan.Web.ViewModels;

namespace Vulcan.Web.ViewComponents
{
    public class PaginatorViewComponent : ViewComponent
    {
        private readonly HttpContext _httpContext;
        private readonly ActionContext _actionContext;
        private readonly LinkGenerator _linkGenerator;

        public PaginatorViewModel PaginatorViewModel { get; private set; }

        public PaginatorViewComponent(
            IHttpContextAccessor httpContextAccessor,
            IActionContextAccessor actionContextAccessor,
            LinkGenerator linkGenerator)
        {
            _actionContext = actionContextAccessor.ActionContext;
            _httpContext = httpContextAccessor.HttpContext;
            _linkGenerator = linkGenerator;
        }

        public IViewComponentResult Invoke(PaginatorViewModel model)
        {
            PaginatorViewModel = model;
            return View(this);
        }

        public string CreateUri(int pageIndex)
        {
            var queryParameters = PaginatorViewModel.QueryParameters;
            var displayName = _actionContext.ActionDescriptor.DisplayName;
            var pageName = displayName.Substring(displayName.LastIndexOf("/") + 1);

            var parametersValues = Activator.CreateInstance(queryParameters.GetType(), new object[] { queryParameters });
            var prop = parametersValues.GetType().GetProperty(nameof(QueryParameters.PageIndex));
            prop.SetValue(parametersValues, pageIndex);

            return _linkGenerator.GetUriByPage(
                _httpContext,
                page: pageName,
                values: parametersValues);
        }
    }
}