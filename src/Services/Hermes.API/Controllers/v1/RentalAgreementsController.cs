﻿using AutoMapper;
using Hermes.API.Helpers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nubles.Core.Application.Dto.Reads;
using Nubles.Core.Application.Dto.Writes;
using Nubles.Core.Application.Parameters;
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
    public class RentalAgreementsController : VersionedApiController
    {
        private readonly PantheonDbContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public RentalAgreementsController(
            PantheonDbContext context,
            ILogger<RentalAgreementsController> logger,
            IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a paginated list of rental agreements
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet(Name = nameof(GetRentalAgreements))]
        public async Task<ActionResult<PagedApiResponse<IEnumerable<RentalAgreementDto>>>> GetRentalAgreements(
            [FromQuery] RentalAgreementParameters parameters)
        {
            // TODO: check performance; maybe create a view?
            var query = _context.RentalAgreements
                                .Include(e => e.ParkingSpace)
                                    .ThenInclude(ps => ps.ParkingSpaceType)
                                .Include(e => e.CustomerRentalAgreements)
                                    .ThenInclude(cra => cra.Customer)
                                .AsQueryable()
                                .AsNoTracking();

            var count = await query.CountAsync();
            var offset = parameters.PageNumber * parameters.PageSize;

            query = query.OrderBy(u => u.Id)
                         .Skip(offset)
                         .Take(parameters.PageSize);

            var entities = await query.ToListAsync();
            var data = _mapper.Map<IEnumerable<RentalAgreementDto>>(entities);

            var pagedResponse =
                new PagedApiResponse<IEnumerable<RentalAgreementDto>>(
                    parameters.PageNumber,
                    parameters.PageSize,
                    count,
                    data
                );

            var pagingHelper = new PagingLinksHelper<IEnumerable<RentalAgreementDto>>(pagedResponse, Url);

            pagedResponse = pagingHelper.GenerateLinks(nameof(GetRentalAgreements), parameters);

            return Ok(pagedResponse);
        }

        /// <summary>
        /// Get a rental agreement by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = nameof(GetRentalAgreement))]
        public async Task<ActionResult<ApiResponse<RentalAgreementDto>>> GetRentalAgreement(int id)
        {
            var rentalAgreement = await _context.RentalAgreements.FindAsync(id);

            if (rentalAgreement == null)
            {
                return NotFound();
            }

            // TODO: check perfomance in SSMS; perhaps a Stored Procedure or View will be better ...

            await _context.Entry(rentalAgreement)
                .Reference(agreement => agreement.ParkingSpace)
                .Query()
                .Include(space => space.ParkingSpaceType)
                .LoadAsync();

            await _context.Entry(rentalAgreement)
                .Collection(agreement => agreement.CustomerRentalAgreements)
                .Query()
                .Include(cra => cra.Customer)
                .LoadAsync();

            var dto = _mapper.Map<RentalAgreementDto>(rentalAgreement);

            dto.Customers = _mapper.Map<ICollection<CustomerDto>>(rentalAgreement.GetCustomers());

            var response = new ApiResponse<RentalAgreementDto>(dto);

            return Ok(response);
        }

        /// <summary>
        /// Update a rental agreement
        /// </summary>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> PatchRentalAgreement(
            int id,
            [FromQuery] int employeeId,
            [FromBody] JsonPatchDocument<UpdateRentalAgreementDto> dtoDoc)
        {
            // TODO: add UserId to request and check if User exists.
            employeeId = 1;

            var rentalAgreement = await _context.RentalAgreements.FindAsync(id);

            if (rentalAgreement == null)
            {
                return NotFound();
            }

            var patchDoc = _mapper.Map<JsonPatchDocument<RentalAgreement>>(dtoDoc);

            patchDoc.ApplyTo(rentalAgreement, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            rentalAgreement.ModifiedBy = employeeId;
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
                    if (!await RentalAgreementExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        DbExceptionHelper.HandleConcurrencyException(ex, rentalAgreement.GetType());
                    }
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Create a new rental agreement
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <param name="addDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ApiResponse<RentalAgreementDto>>> PostRentalAgreement(
            ApiVersion apiVersion,
            AddRentalAgreementDto addDto)
        {
            #region Validation

            // TODO: check if user exists.

            var rat = await _context.RentalAgreementTypes.FindAsync(addDto.RentalAgreementTypeId.Value);
            if (rat == null)
            {
                return NotFound();
            }

            var parkingSpace = await _context
                .ParkingSpaces
                .Include(e => e.ParkingSpaceType)
                .Where(e => e.Id == addDto.ParkingSpaceId.Value)
                .FirstOrDefaultAsync();

            if (parkingSpace == null)
            {
                return NotFound();
            }

            #endregion Validation

            addDto.EmployeeId = 1;
            parkingSpace.IsAvailable = false;
            parkingSpace.ModifiedBy = addDto.EmployeeId.Value;

            var rentalAgreement = _mapper.Map<RentalAgreement>(addDto);
            rentalAgreement.CreatedBy = rentalAgreement.ModifiedBy = addDto.EmployeeId.Value;

            var customer = _mapper.Map<Customer>(addDto.Customer);
            customer.CreatedBy = customer.ModifiedBy = addDto.EmployeeId.Value;
            customer.Vehicles.Add(_mapper.Map<CustomerVehicle>(addDto.Customer.Vehicle));

            rentalAgreement.CustomerRentalAgreements.Add(new CustomerRentalAgreement
            {
                Customer = customer,
                RentalAgreement = rentalAgreement
            });

            _context.RentalAgreements.Add(rentalAgreement);

            await _context.SaveChangesAsync();

            await _context.Entry(customer)
                          .Collection(e => e.Vehicles)
                          .LoadAsync();

            var rentalAgreementDto = _mapper.Map<RentalAgreementDto>(rentalAgreement);
            var customerDto = _mapper.Map<CustomerDto>(customer);
            rentalAgreementDto.Customers.Add(customerDto);

            var response = new ApiResponse<RentalAgreementDto>(rentalAgreementDto);

            return CreatedAtAction(
                actionName: nameof(GetRentalAgreement),
                routeValues: new { id = rentalAgreement.Id, version = apiVersion.ToString() },
                value: response);
        }

        private async Task<bool> RentalAgreementExists(int id)
        {
            return await _context.RentalAgreements.AnyAsync(e => e.Id == id);
        }
    }
}