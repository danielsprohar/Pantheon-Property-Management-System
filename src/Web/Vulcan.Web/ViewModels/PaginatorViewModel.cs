namespace Vulcan.Web.ViewModels
{
    public class PaginatorViewModel
    {
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public object QueryParameters { get; set; }
    }
}