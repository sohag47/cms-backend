using Core.Enums;

namespace API.DTOs.Categories;

public class CategoryListItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public int? ParentId { get; set; }
    public string? ParentName { get; set; }
    public CategoryStatus Status { get; set; }
}

public class CategoryCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public int? ParentId { get; set; }
    public CategoryStatus Status { get; set; } = CategoryStatus.Active;
}

public class CategoryUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public int? ParentId { get; set; }
    public CategoryStatus Status { get; set; } = CategoryStatus.Active;
}
