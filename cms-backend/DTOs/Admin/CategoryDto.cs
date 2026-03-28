using cms_backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace cms_backend.DTOs.Admin;

public class CreateCategoryDto
{
    [Required(ErrorMessage = "Name is required")]
    [MaxLength(100)]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Slug is required")]
    [MaxLength(100)]
    public string? Slug { get; set; }

    public int? ParentId { get; set; }

    [Required(ErrorMessage = "Status is required")]
    public CategoryStatus Status { get; set; } = CategoryStatus.Active;
}

public class EditCategoryDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Slug is required")]
    [MaxLength(100)]
    public string Slug { get; set; } = null!;

    public int? ParentId { get; set; }

    [Required(ErrorMessage = "Status is required")]
    public CategoryStatus Status { get; set; } = CategoryStatus.Active;
}
