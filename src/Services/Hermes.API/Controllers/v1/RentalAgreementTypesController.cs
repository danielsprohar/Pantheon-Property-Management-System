using AutoMapper;
using Hermes.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nubles.Core.Application.Parameters;
using Nubles.Core.Application.Wrappers.Generics;
using Pantheon.Core.Application.Dto.Reads;
using Pantheon.Core.Application.Dto.Writes;
using Pantheon.Core.Domain.Models;
using Pantheon.Infrastructure.Data;
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

        /// <summary>
        /// Get a paginated list of <c>RentalAgreementType</c>s
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get a <c>RentalAgreementType</c> by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Create a new <c>RentalAgreementType</c>
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <param name="addDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ApiResponse<RentalAgreementTypeDto>>> PostRentalAgreementType(
            [FromRoute] ApiVersion apiVersion,
            [FromBody] AddRentalAgreementTypeDto addDto)
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
