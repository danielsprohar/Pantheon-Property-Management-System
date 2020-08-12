using AutoMapper;
using Hermes.API.Application.Pagination;
using Hermes.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nubles.Core.Application.Dto.Reads;
using Nubles.Core.Application.Dto.Writes;
using Nubles.Core.Application.Parameters;
using Nubles.Core.Application.Wrappers.Generics;
using Nubles.Core.Domain.Models;
using Nubles.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Hermes.API.Controllers.v1
{
    [ApiVersion("1.0")]
    public class PaymentMethodsController : VersionedApiController
    {
        private readonly PantheonDbContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public PaymentMethodsController(
            PantheonDbContext context,
            ILogger<PaymentMethodsController> logger,
            IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet(Name = nameof(GetPaymentMethods))]
        public async Task<ActionResult<PaginatedApiResponse<IEnumerable<PaymentMethodDto>>>> GetPaymentMethods(
            [FromQuery] QueryParameters parameters)
        {
            var query = _context.PaymentMethods
                                .AsQueryable()
                                .AsNoTracking();

            var orderedQuery = query.OrderBy(u => u.Id);

            var paginatedList = await PaginatedList<PaymentMethod>
                .CreateAsync(orderedQuery, parameters.PageIndex, parameters.PageSize);

            var data = _mapper.Map<IEnumerable<PaymentMethodDto>>(paginatedList);

            var pagedResponse =
                new PaginatedApiResponse<IEnumerable<PaymentMethodDto>>(
                    data,
                    parameters.PageIndex,
                    parameters.PageSize,
                    paginatedList.Count);

            var pagingHelper = new PagingLinksHelper<IEnumerable<PaymentMethodDto>>(pagedResponse, Url);

            pagedResponse = pagingHelper.GenerateLinks(nameof(GetPaymentMethods), parameters);

            return Ok(pagedResponse);
        }

        [HttpGet("{id}", Name = nameof(GetPaymentMethod))]
        public async Task<ActionResult<ApiResponse<PaymentMethodDto>>> GetPaymentMethod(int id)
        {
            var entity = await _context.PaymentMethods.FindAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<PaymentMethodDto>(entity);
            var response = new ApiResponse<PaymentMethodDto>(dto);

            return Ok(response);
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ApiResponse<PaymentMethodDto>>> PostPaymentMethod(
            ApiVersion apiVersion,
            AddPaymentMethodDto addDto)
        {
            var entity = _mapper.Map<PaymentMethod>(addDto);

            _context.PaymentMethods.Add(entity);

            await _context.SaveChangesAsync();

            var dto = _mapper.Map<PaymentMethodDto>(entity);
            var response = new ApiResponse<PaymentMethodDto>(dto);

            return CreatedAtAction(
                actionName: nameof(GetPaymentMethod),
                routeValues: new { id = entity.Id, version = apiVersion.ToString() },
                value: response);
        }
    }
}
