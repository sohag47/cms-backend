using System.ComponentModel.DataAnnotations;

namespace cms_backend.DTOs.Posts
{
    public class PostCreateDto
    {
        [Required, StringLength(150, MinimumLength = 5)]
        public string Title { get; set; } = null!;

        [Required, StringLength(100)]
        public string Slug { get; set; } = null!;

        [Required]
        public string Content { get; set; } = null!;

        //[Required, EmailAddress]
        //public string Email { get; set; } = null!;

        //[Required, Phone]
        //public string Phone { get; set; } = default!;
        
        //[Required, DataType(DataType.Date)]
        //public DateTime DateOfBirth { get; set; }

        //[Required]
        //public bool IsActive { get; set; }

        //[Required]
        //public IFormFile Photo { get; set; } = default!;
    }
}
