using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes.API.Application.Pagination
{
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