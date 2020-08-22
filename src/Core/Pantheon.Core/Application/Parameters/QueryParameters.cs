using System.Collections.Generic;

namespace Pantheon.Core.Application.Parameters
{
    public class QueryParameters
    {
        public const int MinPageIndex = 1;
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
        /// The following example will return a result set that is sorted in descending order.
        /// <code>
        /// orderBy=createOn_desc
        /// </code>
        /// </remarks>
        /// <example>
        /// orderBy=createOn_desc
        /// </example>
        public string OrderBy { get; set; }

        // TODO: implement orderBy functionality

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

        public IDictionary<string, object> GetRouteValues()
        {
            var routeValuesDictionary = new Dictionary<string, object>();
            var properties = GetType().GetProperties();
            object value = null;

            foreach (var property in properties)
            {
                value = property.GetValue(this);

                if (value != null)
                {
                    routeValuesDictionary.Add(property.Name, value);
                }
            }

            return routeValuesDictionary;
        }
    }
}