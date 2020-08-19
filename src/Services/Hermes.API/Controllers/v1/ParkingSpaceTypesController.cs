using AutoMapper;
using Hermes.API.Application.Pagination;
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
    public class ParkingSpaceTypesController : VersionedApiController
    {
        private readonly PantheonDbContext _context;
        private readonly ILogger<ParkingSpaceTypesController> _logger;
        private readonly IMapper _mapper;

        public ParkingSpaceTypesController(
            PantheonDbContext context,
            ILogger<ParkingSpaceTypesController> logger,
            IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a paginated list of <c>ParkingSpaceType</c>s
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet(Name = nameof(GetParkingSpaceTypes))]
        public async Task<ActionResult<PaginatedApiResponse<IEnumerable<ParkingSpaceTypeDto>>>> GetParkingSpaceTypes(
            [FromQuery] QueryParameters parameters)
        {
            var query = _context.ParkingSpaceTypes
                                .AsQueryable()
                                .AsNoTracking();

            var orderedQuery = query.OrderBy(u => u.Id);

            var paginatedList = await PaginatedList<ParkingSpaceType>
                .CreateAsync(orderedQuery, parameters.PageIndex, parameters.PageSize);

            var data = _mapper.Map<IEnumerable<ParkingSpaceTypeDto>>(paginatedList);

            var pagedResponse =
                new PaginatedApiResponse<IEnumerable<ParkingSpaceTypeDto>>(
                    data,
                    parameters.PageIndex,
                    parameters.PageSize,
                    paginatedList.Count);

            var pagingHelper = new PagingLinksHelper<IEnumerable<ParkingSpaceTypeDto>>(pagedResponse, Url);

            pagedResponse = pagingHelper.GenerateLinks(nameof(GetParkingSpaceTypes), parameters);

            return Ok(pagedResponse);
        }

        /// <summary>
        /// Get a <c>ParkingSpaceType</c> by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = nameof(GetParkingSpaceType))]
        public async Task<ActionResult<ApiResponse<ParkingSpaceTypeDto>>> GetParkingSpaceType(int id)
        {
            var parkingSpaceType = await _context.ParkingSpaceTypes.FindAsync(id);

            if (parkingSpaceType == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<ParkingSpaceTypeDto>(parkingSpaceType);
            var response = new ApiResponse<ParkingSpaceTypeDto>(dto);

            return Ok(response);
        }

        /// <summary>
        /// Create a new <c>ParkingSpaceType</c>
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <param name="addDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ApiResponse<ParkingSpaceTypeDto>>> PostParkingSpaceType(
            [FromRoute] ApiVersion apiVersion,
            [FromBody] AddParkingSpaceTypeDto addDto)
        {
            var entity = _mapper.Map<ParkingSpaceType>(addDto);

            _context.ParkingSpaceTypes.Add(entity);

            await _context.SaveChangesAsync();

            var dto = _mapper.Map<ParkingSpaceTypeDto>(entity);
            var response = new ApiResponse<ParkingSpaceTypeDto>(dto);

            return CreatedAtAction(
                actionName: nameof(GetParkingSpaceType),
                routeValues: new { id = entity.Id, version = apiVersion.ToString() },
                value: response);
        }
    }
}