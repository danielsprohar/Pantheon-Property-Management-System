namespace Nubles.Core.Application.Parameters
{
    public class QueryParameters
    {
        private readonly int MinPageNumber = 0;
        private readonly int MaxPageSize = 50;

        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }

        public QueryParameters()
        {
            PageNumber = MinPageNumber;
            PageSize = MaxPageSize;
        }

        public QueryParameters(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber < MinPageNumber ?
                MinPageNumber :
                pageNumber;

            PageSize = pageSize > MaxPageSize ?
                MaxPageSize :
                pageSize;
        }
    }
}