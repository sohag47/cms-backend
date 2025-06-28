using cms_backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace cms_backend.DTOs.Categories
{
    public class CategoryDropdownQueryDto
    {
        public string? Search { get; set; }
        public int? ParentId { get; set; }
        public CategoryStatus? Status { get; set; }
    }
}
