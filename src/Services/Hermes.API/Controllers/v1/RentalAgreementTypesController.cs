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
    public class RentalAgreementTypesController : VersionedApiController
    {
        private readonly PantheonDbContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public RentalAgreementTypesController(
            PantheonDbContext context,
            ILogger<RentalAgreementTypesController> logger,
            IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet(Name = nameof(GetRentalAgreementTypes))]
        public async Task<ActionResult<PaginatedApiResponse<IEnumerable<RentalAgreementTypeDto>>>> GetRentalAgreementTypes(
            [FromQuery] QueryParameters parameters)
        {
            var query = _context.RentalAgreementTypes
                                .AsQueryable()
                                .AsNoTracking();

            var count = await query.CountAsync();
            var offset = parameters.PageIndex * parameters.PageSize;

            query = query.OrderBy(u => u.Id)
                         .Skip(offset)
                         .Take(parameters.PageSize);

            var entities = await query.ToListAsync();
            var data = _mapper.Map<IEnumerable<RentalAgreementTypeDto>>(entities);

            var pagedResponse =
                new PaginatedApiResponse<IEnumerable<RentalAgreementTypeDto>>(
                    data
,
                    parameters.PageIndex,
                    parameters.PageSize,
                    count);

            var pagingHelper = new PagingLinksHelper<IEnumerable<RentalAgreementTypeDto>>(pagedResponse, Url);

            pagedResponse = pagingHelper.GenerateLinks(nameof(GetRentalAgreementTypes), parameters);

            return Ok(pagedResponse);
        }

        [HttpGet("{id}", Name = nameof(GetRentalAgreementType))]
        public async Task<ActionResult<ApiResponse<RentalAgreementTypeDto>>> GetRentalAgreementType(int id)
        {
            var entity = await _context.RentalAgreementTypes.FindAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<RentalAgreementTypeDto>(entity);
            var response = new ApiResponse<RentalAgreementTypeDto>(dto);

            return Ok(response);
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ApiResponse<RentalAgreementTypeDto>>> PostRentalAgreementType(
            ApiVersion apiVersion,
            AddRentalAgreementTypeDto addDto)
        {
            var entity = _mapper.Map<RentalAgreementType>(addDto);

            _context.RentalAgreementTypes.Add(entity);

            await _context.SaveChangesAsync();

            var dto = _mapper.Map<RentalAgreementTypeDto>(entity);
            var response = new ApiResponse<RentalAgreementTypeDto>(dto);

            return CreatedAtAction(
                actionName: nameof(GetRentalAgreementType),
                routeValues: new { id = entity.Id, version = apiVersion.ToString() },
                value: response);
        }
    }
}
