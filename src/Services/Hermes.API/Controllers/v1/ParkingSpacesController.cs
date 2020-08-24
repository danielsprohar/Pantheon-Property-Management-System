using AutoMapper;
using Hermes.API.Application.Pagination;
using Hermes.API.Extensions;
using Hermes.API.Helpers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pantheon.Core.Application.Dto.Reads;
using Pantheon.Core.Application.Dto.Writes;
using Pantheon.Core.Application.Extensions;
using Pantheon.Core.Application.Parameters;
using Pantheon.Core.Application.Wrappers;
using Pantheon.Core.Application.Wrappers.Generics;
using Pantheon.Core.Domain.Models;
using Pantheon.Identity.Data;
using Pantheon.Identity.Models;
using Pantheon.Infrastructure.Data;
using Pantheon.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hermes.API.Controllers.v1
{
    [ApiVersion("1.0")]
    public class ParkingSpacesController : VersionedApiController
    {
        public ParkingSpacesController(
            PantheonIdentityDbContext identityContext,
            PantheonDbContext context,
            ILogger<ParkingSpacesController> logger,
            IMapper mapper)
                : base(identityContext, context, logger, mapper)
        {
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
                return EntityDoesNotExistResponse<ParkingSpace, int>(id);
            }

            if (!parkingSpace.IsAvailable.Value)
            {
                var message = $"ParkingSpace.Id={id} is currently occupied.";
                _logger.LogInformation(message);
                return UnprocessableEntity(new ApiResponse(message));
            }

            _context.ParkingSpaces.Remove(parkingSpace);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"ParkingSpace.Id={id} was deleted.");

            return NoContent();
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
                return EntityDoesNotExistResponse<ParkingSpace, int>(id);
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

            if (!await EmployeeExistsAsync(addDto.EmployeeId))
            {
                return EntityDoesNotExistResponse<ApplicationUser, Guid>(addDto.EmployeeId);
            }

            if (!await ParkingSpaceTypeExistsAsync(addDto.ParkingSpaceTypeId.Value))
            {
                return EntityDoesNotExistResponse<ParkingSpaceType, int>(addDto.ParkingSpaceTypeId.Value);
            }

            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;

            #endregion Validation

            var entity = _mapper.Map<ParkingSpace>(addDto);

            _context.ParkingSpaces.Add(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"ParkingSpace.Id {entity.Id} was created.");

            var dto = _mapper.Map<ParkingSpaceDto>(entity);
            var response = new ApiResponse<ParkingSpaceDto>(dto);

            return CreatedAtAction(nameof(GetParkingSpace),
                                   new { id = entity.Id, version = apiVersion.ToString() },
                                   response);
        }

        /// <summary>
        /// Update a <c>ParkingSpace</c> by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dtoDoc"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> PatchParkingSpace(
            [FromRoute] int id,
            [FromBody] JsonPatchDocument<UpdateParkingSpaceDto> dtoDoc)
        {
            #region Validation

            Guid employeeId = HttpContext.GetUserId();

            if (!await EmployeeExistsAsync(employeeId))
            {
                return EntityDoesNotExistResponse<ApplicationUser, Guid>(employeeId);
            }

            var space = await _context.ParkingSpaces.FindAsync(id);

            if (space == null)
            {
                return EntityDoesNotExistResponse<ParkingSpace, int>(id);
            }

            #endregion Validation

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
                        return EntityDoesNotExistResponse<ParkingSpace, int>(id);
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
            [FromBody] UpdateParkingSpaceDto dto)
        {
            if (!await EmployeeExistsAsync(dto.EmployeeId))
            {
                return EntityDoesNotExistResponse<ApplicationUser, Guid>(dto.EmployeeId);
            }

            var parkingSpace = await _context.ParkingSpaces.FindAsync(id);
            if (parkingSpace == null)
            {
                return EntityDoesNotExistResponse<ParkingSpace, int>(id);
            }

            _mapper.Map(dto, parkingSpace);

            _context.Entry(parkingSpace).State = EntityState.Modified;

            var saved = false;

            while (!saved)
            {
                try
                {
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"ParkingSpace.Id number {id} was updated.");
                    saved = true;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!await ParkingSpaceExistsAsync(id))
                    {
                        return EntityDoesNotExistResponse<ParkingSpace, int>(id);
                    }
                    else
                    {
                        DbExceptionHelper.HandleConcurrencyException(ex, parkingSpace.GetType());
                    }
                }
            }

            return NoContent();
        }
    }
}