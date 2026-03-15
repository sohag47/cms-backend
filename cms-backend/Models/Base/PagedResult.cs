namespace cms_backend.Models.Base
{
    public class PagedResult<T>
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<T> Data { get; set; } = new List<T>();


        //public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    }
}
