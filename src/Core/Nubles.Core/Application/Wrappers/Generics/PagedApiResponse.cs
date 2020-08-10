using System;

namespace Nubles.Core.Application.Wrappers.Generics
{
    public class PagedApiResponse<T> : ApiResponse<T> where T : class
    {
        public PagedApiResponse(int pageNumber, int pageSize, long count, T data)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Count = count;
            Data = data;
            Message = null;
            Succeeded = true;
            Errors = null;
            TotalPages = (int)Math.Ceiling(count / (decimal)pageSize);
        }

        public int PageNumber { get; private set; }

        public int PageSize { get; private set; }

        public long Count { get; private set; }

        public int TotalPages { get; private set; }

        public string FirstPage { get; set; }

        public string PreviousPage { get; set; }

        public string NextPage { get; set; }

        public string LastPage { get; set; }

        public bool HasPrevious()
        {
            return Count != 0 && PageNumber > 0;
        }

        public bool HasNext()
        {
            // add 1 because pagenumber starts at 0
            return Count != 0 && PageNumber + 1 < TotalPages;
        }
    }
}