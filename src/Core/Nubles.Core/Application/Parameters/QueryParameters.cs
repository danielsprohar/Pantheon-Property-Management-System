namespace Nubles.Core.Application.Parameters
{
    public class QueryParameters
    {
        private readonly int MinPageNumber = 0;
        private readonly int MaxPageSize = 50;

        private int _pageNumber;
        private int _pageSize;

        public QueryParameters()
        {

        }

        public QueryParameters(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public int PageNumber
        {
            get { return _pageNumber; }
            set
            {
                _pageNumber = value < MinPageNumber ?
                MinPageNumber :
                value;
            }
        }

        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = value > MaxPageSize ?
                    MaxPageSize :
                    value;
            }
        }
    }
}