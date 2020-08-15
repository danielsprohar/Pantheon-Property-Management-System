using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes.API.Application.Pagination
{
    /// <summary>
    /// Creates a paginated list of type <c>T</c>.
    /// When a new instance of this object is created
    /// the total number of pages -- 
    /// based on total <c>Count</c> and <c>PageSize</c> --
    /// is calculated and stored as a property. In addition,
    /// the total <c>Count</c>, <c>PageIndex</c>, and <c>PageSize</c>
    /// are stored as properties.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int PageSize { get; set; }

        /// <summary>
        /// The total count in the database
        /// </summary>
        public long TotalCount { get; set; }
        public int TotalPages { get; private set; }

        private PaginatedList(List<T> items, long count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = count;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items);
        }

        public bool HasPreviousPage
        {
            get
            {
                return PageIndex > 1;
            }
        }

        public bool HasNextPage
        {
            get
            {
                return PageIndex < TotalPages;
            }
        }

        /// <summary>
        /// Creates a paginated list of type <c>T</c>.
        /// The total <c>Count</c> is set by making a call to the underlying data source,
        /// e.g., a SQL Server database. 
        /// In addition, the data is also retrieved by making a call to the underlying data source.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static async Task<PaginatedList<T>> CreateAsync(IOrderedQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();

            var items = await source.Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();

            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}