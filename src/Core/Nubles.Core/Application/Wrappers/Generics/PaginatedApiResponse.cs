using System;
using System.Collections.Generic;

namespace Nubles.Core.Application.Wrappers.Generics
{
    public class PaginatedApiResponse<T> : ApiResponse<T>
    {
        public PaginatedApiResponse(T data, int pageIndex, int pageSize, long count)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
            Data = data;
            Message = null;
            Succeeded = true;
            Errors = null;
            TotalPages = (int)Math.Ceiling(count / (decimal)pageSize);
        }

        public int PageIndex { get; private set; }

        public int PageSize { get; private set; }

        public long Count { get; private set; }

        public int TotalPages { get; private set; }

        public string FirstPage { get; set; }

        public string PreviousPage { get; set; }

        public string NextPage { get; set; }

        public string LastPage { get; set; }

        public bool HasPrevious()
        {
            return Count != 0 && PageIndex > 0;
        }

        public bool HasNext()
        {
            // add 1 because pagenumber starts at 0
            return Count != 0 && PageIndex + 1 < TotalPages;
        }
    }
}