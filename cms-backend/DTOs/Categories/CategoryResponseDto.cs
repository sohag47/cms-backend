using cms_backend.DTOs.Users;
using cms_backend.Models;

namespace cms_backend.DTOs.Categories
{
    public class CategoryResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public int ParentId { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        //public UserResponseDto? Author { get; set; }
    }
}
