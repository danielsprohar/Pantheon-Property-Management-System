using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pantheon.Core.Application.Wrappers;
using Pantheon.Identity.Data;
using Pantheon.Infrastructure.Data;
using System;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Hermes.API.Controllers
{
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class VersionedApiController : ControllerBase
    {
        protected readonly PantheonIdentityDbContext _identityContext;
        protected readonly PantheonDbContext _context;
        protected readonly ILogger _logger;
        protected readonly IMapper _mapper;

        public VersionedApiController(
            PantheonIdentityDbContext identityContext,
            PantheonDbContext context,
            ILogger logger,
            IMapper mapper)
        {
            _identityContext = identityContext;
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        protected async Task<bool> CustomerExistsAsync(int id)
        {
            return await _context.Customers
                .AsNoTracking()
                .AnyAsync(e => e.Id == id);
        }

        protected async Task<bool> EmployeeExistsAsync(Guid uid)
        {
            return await _identityContext.Users.AnyAsync(e => e.Id == uid);
        }

        protected async Task<bool> InvoiceStatusExists(int id)
        {
            return await _context.InvoiceStatuses.AnyAsync(e => e.Id == id);
        }

        protected async Task<bool> InvoiceExists(int id)
        {
            return await _context.Invoices.AnyAsync(e => e.Id == id);
        }

        protected async Task<bool> ParkingSpaceExistsAsync(int id)
        {
            return await _context.ParkingSpaces.AnyAsync(e => e.Id == id);
        }

        protected async Task<bool> ParkingSpaceTypeExistsAsync(int id)
        {
            return await _context.ParkingSpaceTypes.AnyAsync(e => e.Id == id);
        }

        protected async Task<bool> PaymentMethodExistsAsync(int id)
        {
            return await _context.PaymentMethods
                .AsNoTracking()
                .AnyAsync(e => e.Id == id);
        }

        protected async Task<bool> RentalAgreementExists(int id)
        {
            return await _context.RentalAgreements.AnyAsync(e => e.Id == id);
        }

        protected string GetRemoteIpAddress()
        {
            return HttpContext.Connection.RemoteIpAddress.ToString();
        }

        /// <summary>
        /// Logs that the entity with the given Id does not exist,
        /// then returns a HTTP 404 Not Found response.
        /// </summary>
        /// <typeparam name="TEntity">The entity type</typeparam>
        /// <typeparam name="TKey">The primary key type</typeparam>
        /// <param name="id">The primary key value</param>
        /// <returns></returns>
        protected ActionResult EntityDoesNotExistResponse<TEntity, TKey>(TKey id)
        {

            var message = $"{typeof(TEntity).Name}.Id {id} does not exist.";
            _logger.LogInformation(message);
            return NotFound(new ApiResponse(message));
        }
    }
}