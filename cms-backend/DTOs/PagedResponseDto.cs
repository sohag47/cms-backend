using cms_backend.Models;

namespace cms_backend.DTOs
{
    public class PagedResponseDto<T>(IEnumerable<T> items, int page, int pageSize, int totalRecords)
    {
        public IEnumerable<T> Items { get; set; } = items;
        public int Page { get; set; } = page;
        public int PageSize { get; set; } = pageSize;
        public int TotalRecords { get; set; } = totalRecords;
        public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);

        public static PagedResponseDto<T> Create(IEnumerable<T> items, int page, int pageSize, int totalRecords)
        {
            return new PagedResponseDto<T>(items, page, pageSize, totalRecords);
        }
    }

}
