namespace Nubles.Core.Application.Wrappers.Generics
{
    public class PagedApiResponse<T> : ApiResponse<T> where T : class
    {
        public int PageNumber { get; private set; }

        public int PageSize { get; private set; }

        public long Count { get; private set; }

        /// <summary>
        /// The url of the previous page.
        /// </summary>
        public string Previous { get; set; }

        /// <summary>
        /// The url of the next page.
        /// </summary>
        public string Next { get; set; }

        public PagedApiResponse(int pageNumber, int pageSize, long count, T data)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Count = count;
            Data = data;
            Message = null;
            Succeeded = true;
            Errors = null;
        }

        // TODO: add HasNext() and HasPrev()
    }
}