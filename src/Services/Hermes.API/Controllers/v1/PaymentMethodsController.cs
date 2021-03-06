﻿using AutoMapper;
using Hermes.API.Application.Pagination;
using Hermes.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pantheon.Core.Application.Dto.Reads;
using Pantheon.Core.Application.Dto.Writes;
using Pantheon.Core.Application.Parameters;
using Pantheon.Core.Application.Wrappers.Generics;
using Pantheon.Core.Domain.Models;
using Pantheon.Identity.Data;
using Pantheon.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Hermes.API.Controllers.v1
{
    [ApiVersion("1.0")]
    public class PaymentMethodsController : VersionedApiController
    {
        public PaymentMethodsController(
            PantheonIdentityDbContext identityContext,
            PantheonDbContext context,
            ILogger<PaymentMethodsController> logger,
            IMapper mapper)
                : base(identityContext, context, logger, mapper)
        {
        }

        /// <summary>
        /// Get a paginated list of <c>PaymentMethod</c>s
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet(Name = nameof(GetPaymentMethods))]
        public async Task<ActionResult<PaginatedApiResponse<IEnumerable<PaymentMethodDto>>>> GetPaymentMethods(
            [FromQuery] QueryParameters parameters)
        {
            var query = _context.PaymentMethods
                                .AsQueryable()
                                .AsNoTracking();

            var orderedQuery = query.OrderBy(u => u.Id);

            var paginatedList = await PaginatedList<PaymentMethod>
                .CreateAsync(orderedQuery, parameters.PageIndex, parameters.PageSize);

            var data = _mapper.Map<IEnumerable<PaymentMethodDto>>(paginatedList);

            var pagedResponse =
                new PaginatedApiResponse<IEnumerable<PaymentMethodDto>>(
                    data,
                    parameters.PageIndex,
                    parameters.PageSize,
                    paginatedList.Count);

            var pagingHelper = new PagingLinksHelper<IEnumerable<PaymentMethodDto>>(pagedResponse, Url);

            pagedResponse = pagingHelper.GenerateLinks(nameof(GetPaymentMethods), parameters);

            return Ok(pagedResponse);
        }

        /// <summary>
        /// Get a <c>PaymentMethod</c> by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = nameof(GetPaymentMethod))]
        public async Task<ActionResult<ApiResponse<PaymentMethodDto>>> GetPaymentMethod(int id)
        {
            var entity = await _context.PaymentMethods.FindAsync(id);

            if (entity == null)
            {
                return EntityDoesNotExistResponse<PaymentMethod, int>(id);
            }

            var dto = _mapper.Map<PaymentMethodDto>(entity);
            var response = new ApiResponse<PaymentMethodDto>(dto);

            return Ok(response);
        }

        /// <summary>
        /// Create a new <c>PaymentMethod</c>
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <param name="addDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ApiResponse<PaymentMethodDto>>> PostPaymentMethod(
            [FromRoute] ApiVersion apiVersion,
            [FromBody] AddPaymentMethodDto addDto)
        {
            var entity = _mapper.Map<PaymentMethod>(addDto);

            _context.PaymentMethods.Add(entity);

            await _context.SaveChangesAsync();

            var dto = _mapper.Map<PaymentMethodDto>(entity);
            var response = new ApiResponse<PaymentMethodDto>(dto);

            _logger.LogInformation($"PaymentMethod.Id {entity.Id} was created.");

            return CreatedAtAction(
                actionName: nameof(GetPaymentMethod),
                routeValues: new { id = entity.Id, version = apiVersion.ToString() },
                value: response);
        }
    }
}