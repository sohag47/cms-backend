using cms_backend.Enums;
using cms_backend.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace cms_backend.Models
{
    public class Category  : AuditableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(150, MinimumLength = 5)]
        public string Name { get; set; } = null!;

        [Required, StringLength(100)]
        public string Slug { get; set; } = null!;

        // Foreign key to User
        public int? ParentId { get; set; }

        public Category? Parent { get; set; }

        public ICollection<Category>? Children { get; set; } = new List<Category>();

        [Required]
        public CategoryStatus Status { get; set; } = CategoryStatus.Active;
    }
}
