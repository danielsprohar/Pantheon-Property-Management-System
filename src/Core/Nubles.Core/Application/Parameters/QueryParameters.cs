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
            _pageIndex = MinPageIndex;
            _pageSize = DefaultPageSize;
        }

        public QueryParameters(int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        /// <summary>
        /// A comma separated string of values.
        /// </summary>
        /// <remarks>
        /// To sort by descending order, 
        ///     append "_desc" to the end of each value.<br/>
        ///     
        /// The following example will return result set that is sorted in descending order.
        /// </remarks>
        /// <example>
        /// orderBy=createOn_desc
        /// </example>
        public string OrderBy { get; set; }

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