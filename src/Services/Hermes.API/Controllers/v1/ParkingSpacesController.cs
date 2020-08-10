﻿using AutoMapper;
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
        public async Task<ActionResult<PagedApiResponse<IEnumerable<ParkingSpaceDto>>>> GetParkingSpaces(
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

            var count = await query.CountAsync();
            var offset = parameters.PageNumber * parameters.PageSize;

            query = query.OrderBy(u => u.Id)
                         .Skip(offset)
                         .Take(parameters.PageSize);

            var unitTypes = await query.ToListAsync();
            var data = _mapper.Map<IEnumerable<ParkingSpaceDto>>(unitTypes);

            var pagedResponse =
                new PagedApiResponse<IEnumerable<ParkingSpaceDto>>(
                    parameters.PageNumber,
                    parameters.PageSize,
                    count,
                    data
                );

            var pagingHelper = new PagingLinksHelper<IEnumerable<ParkingSpaceDto>>(pagedResponse, Url);

            pagedResponse = pagingHelper.GenerateLinks(nameof(GetParkingSpaces), parameters);

            return Ok(pagedResponse);
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

            _context.ParkingSpaces.Remove(parkingSpace);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ParkingSpaceExists(int id)
        {
            return _context.ParkingSpaces.Any(e => e.Id == id);
        }
    }
}