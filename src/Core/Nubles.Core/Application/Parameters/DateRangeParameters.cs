using System;

namespace Nubles.Core.Application.Parameters
{
    public class DateRangeParameters : QueryParameters
    {
        public DateTimeOffset? StartDate { get; set; }

        public DateTimeOffset? EndDate { get; set; }

        public bool GetHasDateFilter()
        {
            return StartDate.HasValue || EndDate.HasValue;
        }

        public bool GetHasUpperAndLowerBound()
        {
            return StartDate.HasValue && EndDate.HasValue;
        }

        public bool GetIsValidDateRange()
        {
            return StartDate <= EndDate;
        }
    }
}