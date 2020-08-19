using AutoMapper;
using Hermes.API.Application.Pagination;
using Hermes.API.Helpers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nubles.Core.Application.Parameters;
using Nubles.Core.Application.Wrappers;
using Nubles.Core.Application.Wrappers.Generics;
using Pantheon.Core.Application.Dto.Reads;
using Pantheon.Core.Application.Dto.Writes;
using Pantheon.Core.Application.Extensions;
using Pantheon.Core.Domain.Models;
using Pantheon.Infrastructure.Data;
using Pantheon.Infrastructure.Helpers;
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

        /// <summary>
        /// Get a paginated list of <c>ParkingSpace</c>s
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet(Name = nameof(GetParkingSpaces))]
        public async Task<ActionResult<PaginatedApiResponse<IEnumerable<ParkingSpaceDto>>>> GetParkingSpaces(
            [FromQuery] ParkingSpaceQueryParameters parameters)
        {
            var query = _context.ParkingSpaces
                                .Include(space => space.ParkingSpaceType)
                                .AsQueryable()
                                .AsNoTracking();

            query = query.BuildSqlQueryFromParameters(parameters);

            var orderedQuery = query.OrderBy(u => u.Id);

            var paginatedList = await PaginatedList<ParkingSpace>
                .CreateAsync(orderedQuery, parameters.PageIndex, parameters.PageSize);

            var data = _mapper.Map<IEnumerable<ParkingSpaceDto>>(paginatedList);

            var paginatedResponse =
                new PaginatedApiResponse<IEnumerable<ParkingSpaceDto>>(
                    data,
                    parameters.PageIndex,
                    parameters.PageSize,
                    paginatedList.TotalCount);

            var pagingHelper = new PagingLinksHelper<IEnumerable<ParkingSpaceDto>>(paginatedResponse, Url);

            paginatedResponse = pagingHelper.GenerateLinks(nameof(GetParkingSpaces), parameters);

            return Ok(paginatedResponse);
        }

        /// <summary>
        /// Get a <c>ParkingSpace</c> by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Create a new <c>ParkingSpace</c>
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <param name="addDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ApiResponse<ParkingSpaceDto>>> PostParkingSpace(
            [FromRoute] ApiVersion apiVersion,
            [FromBody] AddParkingSpaceDto addDto)
        {
            #region Validation

            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            // TODO: check if user exists

            if (!await ParkingSpaceTypeExistsAsync(addDto.ParkingSpaceTypeId.Value))
            {
                return BadRequest(new ApiResponse($"{nameof(ParkingSpaceType)} {addDto.ParkingSpaceTypeId.Value} does not exist."));
            }

            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;

            #endregion Validation

            var entity = _mapper.Map<ParkingSpace>(addDto);

            entity.ModifiedBy = entity.EmployeeId = addDto.EmployeeId.Value;

            _context.ParkingSpaces.Add(entity);

            await _context.SaveChangesAsync();

            var dto = _mapper.Map<ParkingSpaceDto>(entity);

            var response = new ApiResponse<ParkingSpaceDto>(dto);

            return CreatedAtAction(nameof(GetParkingSpace),
                                   new { id = entity.Id, version = apiVersion.ToString() },
                                   response);
        }

        /// <summary>
        /// Delete <c>ParkingSpace</c> by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Update a <c>ParkingSpace</c> by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employeeId"></param>
        /// <param name="dtoDoc"></param>
        /// <returns></returns>
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
                    if (!await ParkingSpaceExistsAsync(id))
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

        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> PutParkingSpace(
            [FromRoute] int id,
            [FromQuery] string employeeId,
            [FromBody] UpdateParkingSpaceDto dto)
        {
            var parkingSpace = await _context.ParkingSpaces.FindAsync(id);
            if (parkingSpace == null)
            {
                _logger.LogInformation($"ParkingSpace with Id number {id} does not exist.");
                return BadRequest();
            }

            _mapper.Map(dto, parkingSpace);

            // TODO: Changed all the modified by data types to string type
            parkingSpace.ModifiedBy = 1;
            parkingSpace.ModifiedOn = System.DateTimeOffset.UtcNow;

            _context.Entry(parkingSpace).State = EntityState.Modified;

            var saved = false;

            while (!saved)
            {
                try
                {
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"ParkingSpace with Id number {id} was updated.");
                    saved = true;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!await ParkingSpaceExistsAsync(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        DbExceptionHelper.HandleConcurrencyException(ex, parkingSpace.GetType());
                    }
                }
            }

            return NoContent();
        }

        private async Task<bool> ParkingSpaceExistsAsync(int id)
        {
            return await _context.ParkingSpaces.AnyAsync(e => e.Id == id);
        }

        private async Task<bool> ParkingSpaceTypeExistsAsync(int id)
        {
            return await _context.ParkingSpaceTypes.AnyAsync(e => e.Id == id);
        }
    }
}