using Nubles.Core.Application.Parameters;
using Nubles.Core.Domain.Models;
using System.Linq;

namespace Nubles.Core.Application.Extensions
{
    public static class QueryParameterExtensions
    {
        public static IQueryable<Invoice> Predicates(
            this IQueryable<Invoice> query,
            InvoiceQueryParameters parameters)
        {
            return query;
        }

        public static IQueryable<ParkingSpace> Predicates(
            this IQueryable<ParkingSpace> query,
            ParkingSpaceQueryParameters parameters)
        {
            return query;
        }

        /// <summary>
        /// Enumerates the <c>RentalAgreementParameters</c> object and
        ///     constructs an object of <c>IQueryable</c> from said object.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static IQueryable<RentalAgreement> Predicates(
            this IQueryable<RentalAgreement> query,
            RentalAgreementQueryParameters parameters)
        {
            if (parameters.EmployeeId.HasValue)
            {
                query = query.Where(e => e.EmployeeId == parameters.EmployeeId.Value);
            }
            if (parameters.IsActive.HasValue)
            {
                if (parameters.IsActive.Value)
                {
                    query = query.Where(e => e.TerminatedOn == null);
                }
                else
                {
                    query = query.Where(e => e.TerminatedOn != null);
                }
            }
            if (parameters.ParkingSpaceId.HasValue)
            {
                query = query.Where(e => e.ParkingSpaceId == parameters.ParkingSpaceId.Value);
            }

            // ==================================================================================
            // Date filters
            // ==================================================================================
            if (parameters.HasSpecificDateFilter())
            {
                if (parameters.HasUpperAndLowerBound())
                {
                    query = query.Where(e =>
                        parameters.StartDate <= e.CreatedOn &&
                        e.CreatedOn <= parameters.EndDate);
                }
                else
                {
                    if (parameters.StartDate.HasValue)
                    {
                        query = query.Where(e => parameters.StartDate <= e.CreatedOn);
                    }
                    else
                    {
                        query = query.Where(e => e.CreatedOn <= parameters.EndDate);
                    }
                }
            }
            else
            {
                if (parameters.HasYearFilter())
                {
                    query = query.Where(e => e.CreatedOn.Year == parameters.Year.Value);
                }
                if (parameters.HasMonthFilter())
                {
                    query = query.Where(e => e.CreatedOn.Month == parameters.Month.Value);
                }
            }

            return query;
        }
    }
}