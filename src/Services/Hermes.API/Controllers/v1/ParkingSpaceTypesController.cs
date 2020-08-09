using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nubles.Core.Application.Dto.Reads;
using Nubles.Core.Application.Dto.Writes;
using Nubles.Core.Application.Wrappers.Generics;
using Nubles.Core.Domain.Models;
using Nubles.Infrastructure.Data;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Hermes.API.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingSpaceTypesController : ControllerBase
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

        // GET: api/ParkingSpaceTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParkingSpaceType>>> GetParkingSpaceTypes()
        {
            return await _context.ParkingSpaceTypes.ToListAsync();
        }

        // GET: api/ParkingSpaceTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ParkingSpaceType>> GetParkingSpaceType(int id)
        {
            var parkingSpaceType = await _context.ParkingSpaceTypes.FindAsync(id);

            if (parkingSpaceType == null)
            {
                return NotFound();
            }



            return parkingSpaceType;
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