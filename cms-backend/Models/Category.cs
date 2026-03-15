using cms_backend.Enums;
using cms_backend.Models.Base;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace cms_backend.Models
{
    public class Category  : AuditableEntity
    {
        public string Name { get; set; } = null!;

        public string Slug { get; set; } = null!;

        // Self reference
        public int? ParentId { get; set; }

        public Category? Parent { get; set; }

        public ICollection<Category> Children { get; set; } = new List<Category>();

        public CategoryStatus Status { get; set; } = CategoryStatus.Active;
    }
}
