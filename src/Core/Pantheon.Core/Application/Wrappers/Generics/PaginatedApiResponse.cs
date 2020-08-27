using Pantheon.Core.Application.Parameters;
using System;

namespace Pantheon.Core.Application.Wrappers.Generics
{
    public class PaginatedApiResponse<T> : ApiResponse<T>
    {
        public PaginatedApiResponse()
        {
        }

        public PaginatedApiResponse(string message) : base(message)
        {
        }

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

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public long Count { get; set; }

        public int TotalPages { get; set; }

        public string FirstPage { get; set; }

        public string PreviousPage { get; set; }

        public string NextPage { get; set; }

        public string LastPage { get; set; }

        public bool HasPrevious()
        {
            return PageIndex > QueryParameters.MinPageIndex;
        }

        public bool HasNext()
        {
            return PageIndex < TotalPages;
        }
    }
}