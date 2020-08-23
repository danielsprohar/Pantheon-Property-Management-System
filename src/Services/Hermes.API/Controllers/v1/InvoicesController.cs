using AutoMapper;
using Hermes.API.Application.Pagination;
using Hermes.API.Helpers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pantheon.Core.Application.Dto.Reads;
using Pantheon.Core.Application.Dto.Writes;
using Pantheon.Core.Application.Extensions;
using Pantheon.Core.Application.Parameters;
using Pantheon.Core.Application.Wrappers;
using Pantheon.Core.Application.Wrappers.Generics;
using Pantheon.Core.Domain.Models;
using Pantheon.Identity.Data;
using Pantheon.Identity.Models;
using Pantheon.Infrastructure.Data;
using Pantheon.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Hermes.API.Controllers.v1
{
    [ApiVersion("1.0")]
    public class InvoicesController : VersionedApiController
    {
        public InvoicesController(
            PantheonIdentityDbContext identityContext,
            PantheonDbContext context,
            ILogger<InvoicesController> logger,
            IMapper mapper)
                : base(identityContext, context, logger, mapper)
        {
        }

        /// <summary>
        /// Get a paginated list of <c>Invoice</c>s
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet(Name = nameof(GetInvoices))]
        public async Task<ActionResult<PaginatedApiResponse<IEnumerable<InvoiceDto>>>> GetInvoices(
            [FromQuery] InvoiceQueryParameters parameters)
        {
            var query = _context.Invoices
                                .Include(e => e.InvoiceStatus)
                                .AsQueryable()
                                .AsNoTracking();

            query = query.BuildSqlQueryFromParameters(parameters);

            var orderedQuery = query.OrderBy(u => u.Id);

            var paginatedList = await PaginatedList<Invoice>
                .CreateAsync(orderedQuery, parameters.PageIndex, parameters.PageSize);

            var data = _mapper.Map<IEnumerable<InvoiceDto>>(paginatedList);

            var paginatedResponse =
                new PaginatedApiResponse<IEnumerable<InvoiceDto>>(
                    data,
                    parameters.PageIndex,
                    parameters.PageSize,
                    paginatedList.TotalCount);

            var pagingHelper = new PagingLinksHelper<IEnumerable<InvoiceDto>>(paginatedResponse, Url);

            paginatedResponse = pagingHelper.GenerateLinks(nameof(GetInvoices), parameters);

            return Ok(paginatedResponse);
        }

        /// <summary>
        /// Get an <c>Invoice</c> by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = nameof(GetInvoice))]
        public async Task<ActionResult<ApiResponse<InvoiceDto>>> GetInvoice(int id)
        {
            var invoice = await _context
                .Invoices
                .Include(e => e.InvoiceStatus)
                .Include(e => e.InvoiceLines)
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();

            if (invoice == null)
            {
                return EntityDoesNotExistResponse<Invoice, int>(id);
            }

            var dto = _mapper.Map<InvoiceDto>(invoice);
            var response = new ApiResponse<InvoiceDto>(dto);

            return Ok(response);
        }

        /// <summary>
        /// Update an <c>Invoice</c> by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="uid">The employee id</param>
        /// <param name="dtoDoc"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> PatchInvoice(
            int id,
            [FromQuery] Guid uid,
            [FromBody] JsonPatchDocument<UpdateInvoiceDto> dtoDoc)
        {
            #region Validation

            if (!await EmployeeExistsAsync(uid))
            {
                return EntityDoesNotExistResponse<ApplicationUser, Guid>(uid);
            }
            var invoice = await _context.Invoices.FindAsync(id);

            if (invoice == null)
            {
                return EntityDoesNotExistResponse<RentalAgreement, int>(id);
            }

            #endregion Validation

            var patchDoc = _mapper.Map<JsonPatchDocument<Invoice>>(dtoDoc);

            patchDoc.ApplyTo(invoice, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            invoice.ModifiedBy = uid;
            var saved = false;

            while (!saved)
            {
                try
                {
                    await _context.SaveChangesAsync();
                    saved = true;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!await InvoiceExists(id))
                    {
                        return EntityDoesNotExistResponse<Invoice, int>(id);
                    }
                    else
                    {
                        DbExceptionHelper.HandleConcurrencyException(ex, invoice.GetType());
                    }
                }
            }

            _logger.LogInformation($"Invoice.Id={id} has been updated.");

            return NoContent();
        }

        /// <summary>
        /// Create a new <c>Invoice</c>
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <param name="addDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ApiResponse<InvoiceDto>>> PostInvoice(
            [FromRoute] ApiVersion apiVersion,
            [FromBody] AddInvoiceDto addDto)
        {
            #region Validation

            if (!await EmployeeExistsAsync(addDto.EmployeeId))
            {
                return EntityDoesNotExistResponse<ApplicationUser, Guid>(addDto.EmployeeId);
            }

            if (!await InvoiceStatusExists(addDto.InvoiceStatusId.Value))
            {
                return EntityDoesNotExistResponse<InvoiceStatus, int>(addDto.InvoiceStatusId.Value);
            }

            if (!await InvoiceExists(addDto.RentalAgreementId.Value))
            {
                return EntityDoesNotExistResponse<RentalAgreement, int>(addDto.RentalAgreementId.Value);
            }

            if (!await IsValidBillingPeriod(addDto))
            {
                var startDate = addDto.BillingPeriodStart.Value.ToString("d", DateTimeFormatInfo.InvariantInfo);
                var endDate = addDto.BillingPeriodEnd.Value.ToString("d", DateTimeFormatInfo.InvariantInfo);
                var message = $"An invoice exists for the billing period: [{startDate}, {endDate}]";
                return UnprocessableEntity(new ApiResponse(message));
            }

            #endregion Validation

            var invoice = _mapper.Map<Invoice>(addDto);

            foreach (var line in invoice.InvoiceLines)
            {
                line.Invoice = invoice;
            }

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Invoice.Id {invoice.Id} was created.");

            var dto = _mapper.Map<InvoiceDto>(invoice);
            var response = new ApiResponse<InvoiceDto>(dto);

            return CreatedAtAction(actionName: nameof(GetInvoice),
                                   routeValues: new { id = invoice.Id, version = apiVersion.ToString() },
                                   value: response);
        }

        #region Helper methods

        /// <summary>
        /// Checks to see if an <c>Invoice</c> exists for the given billing period.
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns></returns>
        private async Task<bool> IsValidBillingPeriod(AddInvoiceDto invoice)
        {
            var start = invoice.DueDate.Value;
            var end = start.AddMonths(1).AddDays(-1);

            return !await _context.Invoices
                                  .AnyAsync(e => e.RentalAgreementId == invoice.RentalAgreementId &&
                                                 e.BillingPeriodStart.Date == start.Date &&
                                                 e.BillingPeriodEnd.Date == end.Date);
        }

        #endregion Helper methods
    }
}