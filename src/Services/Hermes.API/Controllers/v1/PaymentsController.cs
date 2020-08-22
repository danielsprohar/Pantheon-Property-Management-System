using AutoMapper;
using Hermes.API.Application.Pagination;
using Hermes.API.Helpers;
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
using Pantheon.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Hermes.API.Controllers.v1
{
    [ApiVersion("1.0")]
    public class PaymentsController : VersionedApiController
    {
        private readonly PantheonDbContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public PaymentsController(
            PantheonDbContext context,
            ILogger<PaymentsController> logger,
            IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet(Name = nameof(GetPayments))]
        public async Task<ActionResult<PaginatedApiResponse<PaymentDto>>> GetPayments(
            [FromQuery] PaymentQueryParameters parameters)
        {
            var query = _context.Payments
                                .Include(e => e.PaymentMethod)
                                .AsQueryable()
                                .AsNoTracking();

            query = query.BuildSqlQueryFromParameters(parameters);

            var orderedQuery = query.OrderBy(u => u.Id);

            var paginatedList = await PaginatedList<Payment>
                .CreateAsync(orderedQuery, parameters.PageIndex, parameters.PageSize);

            var data = _mapper.Map<IEnumerable<PaymentDto>>(paginatedList);

            var pagedResponse =
                new PaginatedApiResponse<IEnumerable<PaymentDto>>(
                    data,
                    parameters.PageIndex,
                    parameters.PageSize,
                    paginatedList.Count);

            var pagingHelper = new PagingLinksHelper<IEnumerable<PaymentDto>>(pagedResponse, Url);

            pagedResponse = pagingHelper.GenerateLinks(nameof(GetPayments), parameters);

            return Ok(pagedResponse);
        }

        [HttpGet("{id}", Name = nameof(GetPayment))]
        public async Task<ActionResult<ApiResponse<PaymentDto>>> GetPayment(int id)
        {
            var payment = await _context.Payments.FindAsync(id);

            if (payment == null)
            {
                var message = $"Payment with Id={id} does not exist.";
                return NotFound(new ApiResponse(message));
            }

            var dto = _mapper.Map<PaymentDto>(payment);

            return Ok(new ApiResponse<PaymentDto>(dto));
        }

        // POST: api/v1/invoices/1/payments
        [HttpPost("~/api/v{version:apiVersion}/invoices/{invoiceId}/payments")]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ApiResponse<PaymentDto>>> PostInvoicePayment(
            [FromRoute] int invoiceId,
            [FromRoute] ApiVersion apiVersion,
            [FromBody] AddPaymentDto addDto)
        {
            #region Validation

            // TODO: check if user exists.
            if (invoiceId != addDto.InvoiceId.Value)
            {
                var message = $"The invoice id in the url does not match the invoice id in the request body.";
                return BadRequest(new ApiResponse(message));
            }

            if (!await CustomerExistsAsync(addDto.CustomerId.Value))
            {
                var message = $"Customer with Id={addDto.CustomerId.Value} does not exist.";
                return NotFound(new ApiResponse(message));
            }

            if (!await PaymentMethodExistsAsync(addDto.PaymentMethodId.Value))
            {
                var message = $"Payment Method with Id={addDto.InvoiceId.Value} does not exist.";
                return NotFound(new ApiResponse(message));
            }

            var invoice = await _context.Invoices
                                        .Include(e => e.InvoiceStatus)
                                        .Include(e => e.InvoiceLines)
                                        .Where(e => e.Id == invoiceId)
                                        .FirstOrDefaultAsync();

            if (invoice == null)
            {
                var message = $"Invoice with Id={addDto.InvoiceId.Value} does not exist.";
                return NotFound(new ApiResponse(message));
            }
            if (invoice.InvoiceStatus.Description != InvoiceStatus.Status.AwaitingPayment &&
                invoice.InvoiceStatus.Description != InvoiceStatus.Status.Partial &&
                invoice.InvoiceStatus.Description != InvoiceStatus.Status.PastDue)
            {
                var message = $"Invoice with Id={addDto.InvoiceId.Value} does not require a payment.";
                return UnprocessableEntity(new ApiResponse(message));
            }

            #endregion Validation

            decimal invoiceTotal = invoice.InvoiceLines
                                      .Aggregate(0M,
                                      (sum, invoiceLine) =>
                                      {
                                          sum += invoiceLine.Total;
                                          return sum;
                                      });

            var payment = _mapper.Map<Payment>(addDto);

            await UpdateInvoiceStatusAsync(invoice, payment.Amount, invoiceTotal);

            _context.Payments.Add(payment);

            _context.InvoicePayments.Add(new InvoicePayment
            {
                InvoiceId = invoiceId,
                Payment = payment
            });

            await _context.SaveChangesAsync();

            var dto = _mapper.Map<PaymentDto>(payment);

            var response = new ApiResponse<PaymentDto>(dto);

            return CreatedAtAction(
                actionName: nameof(GetPayment),
                routeValues: new { id = payment.Id, version = apiVersion.ToString() },
                value: response);
        }

        // =========================================================================
        // Helper methods
        // =========================================================================

        private async Task UpdateInvoiceStatusAsync(Invoice invoice, decimal paymentAmount, decimal invoiceTotal)
        {
            int invoiceStatusId;

            if (paymentAmount >= invoiceTotal)
            {
                invoiceStatusId = await _context.InvoiceStatuses
                                           .Where(e => e.Description == InvoiceStatus.Status.Paid)
                                           .Select(e => e.Id)
                                           .FirstOrDefaultAsync();
            }
            else
            {
                invoiceStatusId = await _context.InvoiceStatuses
                                           .Where(e => e.Description == InvoiceStatus.Status.Partial)
                                           .Select(e => e.Id)
                                           .FirstOrDefaultAsync();
            }

            if (invoiceStatusId != default)
            {
                invoice.InvoiceStatusId = invoiceStatusId;
            }
            else
            {
                // TODO: throw an error
            }
        }

        private async Task<bool> CustomerExistsAsync(int id)
        {
            return await _context.Customers
                .AsNoTracking()
                .AnyAsync(e => e.Id == id);
        }

        private async Task<bool> PaymentMethodExistsAsync(int id)
        {
            return await _context.PaymentMethods
                .AsNoTracking()
                .AnyAsync(e => e.Id == id);
        }
    }
}