using cms_backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace cms_backend.DTOs.Categories;

public class CategoryCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public int? ParentId { get; set; }
    public CategoryStatus Status { get; set; }
    //public CategoryStatus Status { get; set; } = CategoryStatus.Active;
}

