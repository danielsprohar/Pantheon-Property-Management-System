using AutoMapper;
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
    [Produces(MediaTypeNames.Application.Json)]
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

        [HttpGet]
        public async Task<ActionResult<PagedApiResponse<IEnumerable<ParkingSpaceTypeDto>>>> GetParkingSpaceTypes(
            [FromQuery] QueryParameters parameters)
        {
            var query = _context.ParkingSpaceTypes
                                .AsQueryable()
                                .AsNoTracking();

            var count = await query.CountAsync();
            var offset = parameters.PageNumber * parameters.PageSize;

            query = query.OrderBy(u => u.Id)
                         .Skip(offset)
                         .Take(parameters.PageSize);

            var unitTypes = await query.ToListAsync();
            var data = _mapper.Map<IEnumerable<ParkingSpaceTypeDto>>(unitTypes);

            var pagedResponse =
                new PagedApiResponse<IEnumerable<ParkingSpaceTypeDto>>(
                    parameters.PageNumber,
                    parameters.PageSize,
                    count,
                    data
                );

            return Ok(pagedResponse);
        }

        [HttpGet("{id}")]
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

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ApiResponse<ParkingSpaceTypeDto>>> PostParkingSpaceType(
            ApiVersion apiVersion,
            AddParkingSpaceTypeDto addDto)
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