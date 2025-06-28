using cms_backend.DTOs.Users;
using cms_backend.Models;

namespace cms_backend.DTOs.Categories
{
    public class DropdownResponseDto
    {
        public int Value { get; set; }
        public string Label { get; set; } = null!;
    }
}
