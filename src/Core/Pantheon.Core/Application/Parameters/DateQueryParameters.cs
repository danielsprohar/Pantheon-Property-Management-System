using System;

namespace Nubles.Core.Application.Parameters
{
    public class DateQueryParameters : QueryParameters
    {
        public DateTimeOffset? StartDate { get; set; }

        public DateTimeOffset? EndDate { get; set; }

        public DateTimeOffset? Date { get; set; }

        public int? Year { get; set; }

        public int? Month { get; set; }

        public bool HasSpecificDateFilter()
        {
            return StartDate.HasValue || EndDate.HasValue;
        }

        public bool HasUpperAndLowerBound()
        {
            return StartDate.HasValue && EndDate.HasValue;
        }

        public bool IsValidDateInterval()
        {
            return StartDate <= EndDate;
        }

        public bool HasYearFilter()
        {
            return Year.HasValue;
        }

        public bool HasMonthFilter()
        {
            return Month.HasValue;
        }
    }
}