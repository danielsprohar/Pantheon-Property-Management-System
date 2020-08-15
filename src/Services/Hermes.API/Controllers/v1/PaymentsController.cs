using AutoMapper;
using Hermes.API.Application.Pagination;
using Hermes.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nubles.Core.Application.Dto.Reads;
using Nubles.Core.Application.Extensions;
using Nubles.Core.Application.Parameters;
using Nubles.Core.Application.Wrappers;
using Nubles.Core.Application.Wrappers.Generics;
using Nubles.Core.Domain.Models;
using Nubles.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
