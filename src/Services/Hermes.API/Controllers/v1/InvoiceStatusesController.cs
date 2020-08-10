using AutoMapper;
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
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Hermes.API.Controllers.v1
{
    [ApiVersion("1.0")]
    public class InvoiceStatusesController : VersionedApiController
    {
        private readonly PantheonDbContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public InvoiceStatusesController(
            PantheonDbContext context,
            ILogger<InvoiceStatusesController> logger,
            IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet(Name = nameof(GetInvoiceStatuses))]
        public async Task<ActionResult<PagedApiResponse<IEnumerable<InvoiceStatusDto>>>> GetInvoiceStatuses(
            [FromQuery] QueryParameters parameters)
        {
            var query = _context.InvoiceStatuses
                                .AsQueryable()
                                .AsNoTracking();

            var count = await query.CountAsync();
            var offset = parameters.PageNumber * parameters.PageSize;

            query = query.OrderBy(u => u.Id)
                         .Skip(offset)
                         .Take(parameters.PageSize);

            var entities = await query.ToListAsync();
            var data = _mapper.Map<IEnumerable<InvoiceStatusDto>>(entities);

            var pagedResponse =
                new PagedApiResponse<IEnumerable<InvoiceStatusDto>>(
                    parameters.PageNumber,
                    parameters.PageSize,
                    count,
                    data
                );

            var pagingHelper = new PagingLinksHelper<IEnumerable<InvoiceStatusDto>>(pagedResponse, Url);

            pagedResponse = pagingHelper.GenerateLinks(nameof(GetInvoiceStatuses), parameters);

            return Ok(pagedResponse);
        }

        [HttpGet("{id}", Name = nameof(GetInvoiceStatus))]
        public async Task<ActionResult<ApiResponse<InvoiceStatusDto>>> GetInvoiceStatus(int id)
        {
            var entity = await _context.InvoiceStatuses.FindAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<InvoiceStatusDto>(entity);
            var response = new ApiResponse<InvoiceStatusDto>(dto);

            return Ok(response);
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ApiResponse<InvoiceStatusDto>>> PostParkingSpaceType(
            ApiVersion apiVersion,
            AddInvoiceStatusDto addDto)
        {
            var entity = _mapper.Map<InvoiceStatus>(addDto);

            _context.InvoiceStatuses.Add(entity);

            await _context.SaveChangesAsync();

            var dto = _mapper.Map<InvoiceStatusDto>(entity);
            var response = new ApiResponse<InvoiceStatusDto>(dto);

            return CreatedAtAction(
                actionName: nameof(GetInvoiceStatus),
                routeValues: new { id = entity.Id, version = apiVersion.ToString() },
                value: response);
        }
    }
}