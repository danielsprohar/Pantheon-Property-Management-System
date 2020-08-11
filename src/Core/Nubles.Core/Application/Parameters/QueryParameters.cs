namespace Nubles.Core.Application.Parameters
{
    public class QueryParameters
    {
        private readonly int MinPageIndex = 1;
        private readonly int MaxPageSize = 50;
        private readonly int DefaultPageSize = 30;

        private int _pageIndex;
        private int _pageSize;

        public QueryParameters()
        {
            _pageIndex = 0;
            _pageSize = DefaultPageSize;
        }

        public QueryParameters(int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public int PageIndex
        {
            get { return _pageIndex; }
            set
            {
                _pageIndex = value < MinPageIndex ?
                MinPageIndex :
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