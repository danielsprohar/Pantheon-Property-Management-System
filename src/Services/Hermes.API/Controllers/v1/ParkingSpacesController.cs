using AutoMapper;
using Hermes.API.Application.Pagination;
using Hermes.API.Helpers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nubles.Core.Application.Dto.Reads;
using Nubles.Core.Application.Dto.Writes;
using Nubles.Core.Application.Parameters;
using Nubles.Core.Application.Wrappers;
using Nubles.Core.Application.Wrappers.Generics;
using Nubles.Core.Domain.Models;
using Nubles.Infrastructure.Data;
using Nubles.Infrastructure.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Hermes.API.Controllers.v1
{
    [ApiVersion("1.0")]
    public class ParkingSpacesController : VersionedApiController
    {
        private readonly PantheonDbContext _context;
        private readonly ILogger<ParkingSpacesController> _logger;
        private readonly IMapper _mapper;

        public ParkingSpacesController(
            PantheonDbContext context,
            ILogger<ParkingSpacesController> logger,
            IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet(Name = nameof(GetParkingSpaces))]
        public async Task<ActionResult<PaginatedApiResponse<IEnumerable<ParkingSpaceDto>>>> GetParkingSpaces(
            [FromQuery] ParkingSpaceParameters parameters)
        {
            var query = _context.ParkingSpaces
                                .AsQueryable()
                                .AsNoTracking();

            if (parameters.IsAvailable.HasValue)
            {
                query = query.Where(e => e.IsAvailable == parameters.IsAvailable);
            }
            if (parameters.Amps.HasValue)
            {
                query = query.Where(e => e.Amps == parameters.Amps);
            }

            var orderedQuery = query.OrderBy(u => u.Id);

            var paginatedList = await PaginatedList<ParkingSpace>
                .CreateAsync(orderedQuery, parameters.PageIndex, parameters.PageSize);

            var data = _mapper.Map<IEnumerable<ParkingSpaceDto>>(paginatedList);

            var paginatedResponse =
                new PaginatedApiResponse<IEnumerable<ParkingSpaceDto>>(
                    data,
                    parameters.PageIndex,
                    parameters.PageSize,
                    paginatedList.Count);

            var pagingHelper = new PagingLinksHelper<IEnumerable<ParkingSpaceDto>>(paginatedResponse, Url);

            paginatedResponse = pagingHelper.GenerateLinks(nameof(GetParkingSpaces), parameters);

            return Ok(paginatedResponse);
        }

        [HttpGet("{id}", Name = nameof(GetParkingSpace))]
        public async Task<ActionResult<ApiResponse<ParkingSpaceDto>>> GetParkingSpace(int id)
        {
            var parkingSpace = await _context
                .ParkingSpaces
                .Include(e => e.ParkingSpaceType)
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();

            if (parkingSpace == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<ParkingSpaceDto>(parkingSpace);
            var response = new ApiResponse<ParkingSpaceDto>(dto);

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ParkingSpaceDto>>> PostParkingSpace(
            ApiVersion apiVersion,
            AddParkingSpaceDto addDto)
        {
            var entity = _mapper.Map<ParkingSpace>(addDto);

            _context.ParkingSpaces.Add(entity);
            await _context.SaveChangesAsync();

            var dto = _mapper.Map<ParkingSpaceDto>(entity);
            var response = new ApiResponse<ParkingSpaceDto>(dto);

            return CreatedAtAction(nameof(GetParkingSpace),
                                   new { id = entity.Id, version = apiVersion.ToString() },
                                   response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParkingSpace(int id)
        {
            var parkingSpace = await _context.ParkingSpaces.FindAsync(id);

            if (parkingSpace == null)
            {
                return NotFound();
            }

            if (!parkingSpace.IsAvailable.Value)
            {
                var errorMessage = "This parking space is currently occupied.";
                return UnprocessableEntity(new ApiResponse(errorMessage));
            }

            _context.ParkingSpaces.Remove(parkingSpace);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> PatchParkingSpace(
            int id,
            [FromQuery] int employeeId,
            [FromBody] JsonPatchDocument<UpdateParkingSpaceDto> dtoDoc)
        {
            // TODO: add UserId to request and check if User exists.
            employeeId = 1;

            var space = await _context.ParkingSpaces.FindAsync(id);

            if (space == null)
            {
                return NotFound();
            }

            var patchDoc = _mapper.Map<JsonPatchDocument<ParkingSpace>>(dtoDoc);

            patchDoc.ApplyTo(space, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            space.ModifiedBy = employeeId;
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
                    if (!await ParkingSpaceExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        DbExceptionHelper.HandleConcurrencyException(ex, space.GetType());
                    }
                }
            }

            return NoContent();
        }

        private async Task<bool> ParkingSpaceExists(int id)
        {
            return await _context.ParkingSpaces.AnyAsync(e => e.Id == id);
        }
    }
}