using Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Admin.Models.Categories;

public class CategoryFormViewModel
{
    public int? Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Slug { get; set; } = string.Empty;

    public int? ParentId { get; set; }

    [Required]
    public CategoryStatus Status { get; set; } = CategoryStatus.Active;
}
