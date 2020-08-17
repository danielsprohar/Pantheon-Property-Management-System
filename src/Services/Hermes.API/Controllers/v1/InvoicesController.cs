using AutoMapper;
using Hermes.API.Application.Pagination;
using Hermes.API.Helpers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nubles.Core.Application.Dto.Reads;
using Nubles.Core.Application.Dto.Writes;
using Nubles.Core.Application.Extensions;
using Nubles.Core.Application.Parameters;
using Nubles.Core.Application.Wrappers;
using Nubles.Core.Application.Wrappers.Generics;
using Nubles.Core.Domain.Models;
using Nubles.Infrastructure.Data;
using Nubles.Infrastructure.Helpers;
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
        private readonly PantheonDbContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public InvoicesController(
            PantheonDbContext context,
            ILogger<InvoicesController> logger,
            IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
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
                return NotFound();
            }

            var dto = _mapper.Map<InvoiceDto>(invoice);
            var response = new ApiResponse<InvoiceDto>(dto);

            return Ok(response);
        }

        /// <summary>
        /// Update an <c>Invoice</c> by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employeeId"></param>
        /// <param name="dtoDoc"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ApiResponse>> PatchInvoice(
            int id,
            [FromQuery] int employeeId,
            [FromBody] JsonPatchDocument<UpdateInvoiceDto> dtoDoc)
        {
            // TODO: add UserId to request and check if User exists.
            employeeId = 1;

            var rentalAgreement = await _context.Invoices.FindAsync(id);

            if (rentalAgreement == null)
            {
                return NotFound();
            }

            var patchDoc = _mapper.Map<JsonPatchDocument<Invoice>>(dtoDoc);

            patchDoc.ApplyTo(rentalAgreement, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            rentalAgreement.ModifiedBy = employeeId;
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
                    if (!await RentalAgreementExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        DbExceptionHelper.HandleConcurrencyException(ex, rentalAgreement.GetType());
                    }
                }
            }

            return Ok(new ApiResponse($"Invoice with Id={id} has been updated.", true));
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

            // TODO: check is Employee exists
            if (!await InvoiceStatusExists(addDto.InvoiceStatusId.Value))
            {
                var message = $"InvoiceStatus {addDto.InvoiceStatusId.Value} does not exist.";
                return NotFound(new ApiResponse(message));
            }

            if (!await RentalAgreementExists(addDto.RentalAgreementId.Value))
            {
                var message = $"RentalAgreement {addDto.RentalAgreementId.Value} does not exist.";
                return NotFound(new ApiResponse(message));
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

            var dto = _mapper.Map<InvoiceDto>(invoice);
            var response = new ApiResponse<InvoiceDto>(dto);

            return CreatedAtAction(actionName: nameof(GetInvoice),
                                   routeValues: new { id = invoice.Id, version = apiVersion.ToString() },
                                   value: response);
        }

        // =========================================================================
        // Helper methods
        // =========================================================================

        private async Task<bool> InvoiceStatusExists(int id)
        {
            return await _context.InvoiceStatuses.AnyAsync(e => e.Id == id);
        }

        private async Task<bool> RentalAgreementExists(int id)
        {
            return await _context.RentalAgreements.AnyAsync(e => e.Id == id);
        }

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
    }
}