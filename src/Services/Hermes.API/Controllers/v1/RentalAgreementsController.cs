using AutoMapper;
using Hermes.API.Application.Pagination;
using Hermes.API.Helpers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pantheon.Core.Application.Dto.Reads;
using Pantheon.Core.Application.Dto.Writes;
using Pantheon.Core.Application.Extensions;
using Pantheon.Core.Application.Parameters;
using Pantheon.Core.Application.Wrappers.Generics;
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
        /// Get a paginated list of <c>RentalAgreement</c>s
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet(Name = nameof(GetRentalAgreements))]
        public async Task<ActionResult<PaginatedApiResponse<IEnumerable<RentalAgreementDto>>>> GetRentalAgreements(
            [FromQuery] RentalAgreementQueryParameters parameters)
        {
            // TODO: check performance; maybe create a view?
            var query = _context.RentalAgreements
                                .Include(e => e.ParkingSpace)
                                    .ThenInclude(ps => ps.ParkingSpaceType)
                                .Include(e => e.CustomerRentalAgreements)
                                    .ThenInclude(cra => cra.Customer)
                                .AsQueryable()
                                .AsNoTracking();

            query = query.BuildSqlQueryFromParameters(parameters);

            var orderedQuery = query.OrderBy(u => u.Id);

            var paginatedList = await PaginatedList<RentalAgreement>
                .CreateAsync(orderedQuery, parameters.PageIndex, parameters.PageSize);

            var data = _mapper.Map<IEnumerable<RentalAgreementDto>>(paginatedList);

            var pagedResponse =
                new PaginatedApiResponse<IEnumerable<RentalAgreementDto>>(
                    data,
                    parameters.PageIndex,
                    parameters.PageSize,
                    paginatedList.Count);

            var pagingHelper = new PagingLinksHelper<IEnumerable<RentalAgreementDto>>(pagedResponse, Url);

            pagedResponse = pagingHelper.GenerateLinks(nameof(GetRentalAgreements), parameters);

            return Ok(pagedResponse);
        }

        /// <summary>
        /// Get a <c>RentalAgreement</c> by Id
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
        /// Update a <c>RentalAgreement</c> by Id
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
        /// Create a new <c>RentalAgreement</c>
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <param name="addDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ApiResponse<RentalAgreementDto>>> PostRentalAgreement(
            [FromRoute] ApiVersion apiVersion,
            [FromBody] AddRentalAgreementDto addDto)
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
            rentalAgreement.EmployeeId = rentalAgreement.ModifiedBy = addDto.EmployeeId.Value;

            var customer = _mapper.Map<Customer>(addDto.Customer);
            customer.EmployeeId = customer.ModifiedBy = addDto.EmployeeId.Value;
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