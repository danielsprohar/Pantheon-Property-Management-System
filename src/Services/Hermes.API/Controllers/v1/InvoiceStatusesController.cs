using AutoMapper;
using Hermes.API.Application.Pagination;
using Hermes.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pantheon.Core.Application.Dto.Reads;
using Pantheon.Core.Application.Dto.Writes;
using Pantheon.Core.Application.Parameters;
using Pantheon.Core.Application.Wrappers;
using Pantheon.Core.Application.Wrappers.Generics;
using Pantheon.Core.Domain.Models;
using Pantheon.Identity.Data;
using Pantheon.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Hermes.API.Controllers.v1
{
    [ApiVersion("1.0")]
    public class InvoiceStatusesController : VersionedApiController
    {
        public InvoiceStatusesController(
            ApplicationIdentityDbContext identityContext,
            PantheonDbContext context,
            ILogger<InvoiceStatusesController> logger,
            IMapper mapper)
                : base(identityContext, context, logger, mapper)
        {
        }

        /// <summary>
        /// Get a paginated list of <c>InvoiceStatus</c>s
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet(Name = nameof(GetInvoiceStatuses))]
        public async Task<ActionResult<PaginatedApiResponse<IEnumerable<InvoiceStatusDto>>>> GetInvoiceStatuses(
            [FromQuery] QueryParameters parameters)
        {
            var query = _context.InvoiceStatuses
                                .AsQueryable()
                                .AsNoTracking();

            var orderedQuery = query.OrderBy(u => u.Id);

            var paginatedList = await PaginatedList<InvoiceStatus>
                .CreateAsync(orderedQuery, parameters.PageIndex, parameters.PageSize);

            var data = _mapper.Map<IEnumerable<InvoiceStatusDto>>(paginatedList);

            var pagedResponse =
                new PaginatedApiResponse<IEnumerable<InvoiceStatusDto>>(
                    data,
                    parameters.PageIndex,
                    parameters.PageSize,
                    paginatedList.Count);

            var pagingHelper = new PagingLinksHelper<IEnumerable<InvoiceStatusDto>>(pagedResponse, Url);

            pagedResponse = pagingHelper.GenerateLinks(nameof(GetInvoiceStatuses), parameters);

            return Ok(pagedResponse);
        }

        /// <summary>
        /// Get an <c>InvoiceStatus</c> by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = nameof(GetInvoiceStatus))]
        public async Task<ActionResult<ApiResponse<InvoiceStatusDto>>> GetInvoiceStatus(int id)
        {
            var entity = await _context.InvoiceStatuses.FindAsync(id);

            if (entity == null)
            {
                return InvoiceStatusDoesNotExistResponse(id);
            }

            var dto = _mapper.Map<InvoiceStatusDto>(entity);
            var response = new ApiResponse<InvoiceStatusDto>(dto);

            return Ok(response);
        }

        /// <summary>
        /// Create a new <c>InvoiceStatus</c>
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <param name="addDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ApiResponse<InvoiceStatusDto>>> PostInvoiceStatus(
            [FromRoute] ApiVersion apiVersion,
            [FromBody] AddInvoiceStatusDto addDto)
        {
            var entity = _mapper.Map<InvoiceStatus>(addDto);

            _context.InvoiceStatuses.Add(entity);

            await _context.SaveChangesAsync();

            var dto = _mapper.Map<InvoiceStatusDto>(entity);
            var response = new ApiResponse<InvoiceStatusDto>(dto);

            _logger.LogInformation($"InvoiceStatus.Id {entity.Id} was created.");

            return CreatedAtAction(
                actionName: nameof(GetInvoiceStatus),
                routeValues: new { id = entity.Id, version = apiVersion.ToString() },
                value: response);
        }

        #region Helper methods

        private ActionResult InvoiceStatusDoesNotExistResponse(int id)
        {
            var message = $"InvoiceStatus.Id {id} does not exist.";
            _logger.LogInformation(message);
            return NotFound(new ApiResponse(message));
        }

        #endregion Helper methods
    }
}