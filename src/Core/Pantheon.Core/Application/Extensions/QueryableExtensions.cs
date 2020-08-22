using Pantheon.Core.Application.Parameters;
using Pantheon.Core.Domain.Base;
using Pantheon.Core.Domain.Models;
using System.Linq;

namespace Pantheon.Core.Application.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<Invoice> BuildSqlQueryFromParameters(
            this IQueryable<Invoice> query,
            InvoiceQueryParameters parameters)
        {
            query = query.BuildDateQueryParameters(parameters);

            if (parameters.EmployeeId.HasValue)
            {
                query = query.Where(e => e.EmployeeId == parameters.EmployeeId.Value);
            }
            if (parameters.InvoiceStatusId.HasValue)
            {
                query = query.Where(e => e.InvoiceStatusId == parameters.InvoiceStatusId.Value);
            }
            if (parameters.DueDate.HasValue)
            {
                query = query.Where(e => e.DueDate.Date == parameters.DueDate.Value.Date);
            }

            return query;
        }

        public static IQueryable<ParkingSpace> BuildSqlQueryFromParameters(
            this IQueryable<ParkingSpace> query,
            ParkingSpaceQueryParameters parameters)
        {
            if (parameters.IsAvailable.HasValue)
            {
                query = query.Where(e => e.IsAvailable == parameters.IsAvailable);
            }
            if (parameters.Amps.HasValue)
            {
                query = query.Where(e => e.Amps == parameters.Amps);
            }

            return query;
        }

        public static IQueryable<Payment> BuildSqlQueryFromParameters(
            this IQueryable<Payment> query,
            PaymentQueryParameters parameters)
        {
            query = query.BuildDateQueryParameters(parameters);

            if (parameters.CustomerId.HasValue)
            {
                query = query.Where(e => e.CustomerId == parameters.CustomerId.Value);
            }
            if (parameters.IsRefund.HasValue)
            {
                query = query.Where(e => e.IsRefund.Value == parameters.IsRefund.Value);
            }
            if (parameters.PaymentMethodId.HasValue)
            {
                query = query.Where(e => e.PaymentMethodId == parameters.PaymentMethodId);
            }

            return query;
        }

        /// <summary>
        /// Enumerates the <c>RentalAgreementParameters</c> object and
        ///     constructs a LINQ query from said object.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static IQueryable<RentalAgreement> BuildSqlQueryFromParameters(
            this IQueryable<RentalAgreement> query,
            RentalAgreementQueryParameters parameters)
        {
            query = query.BuildDateQueryParameters(parameters);

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

            return query;
        }

        private static IQueryable<T> BuildDateQueryParameters<T>(
            this IQueryable<T> query,
            DateQueryParameters parameters) where T : AuditableEntity
        {
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