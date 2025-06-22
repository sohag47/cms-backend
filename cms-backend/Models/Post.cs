using cms_backend.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace cms_backend.Models
{
    public class Post : AuditableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(150, MinimumLength = 5)]
        public string Title { get; set; } = null!;

        [Required, StringLength(100)]
        public string Slug { get; set; } = null!;

        [Required]
        public string Content { get; set; } = null!;


        // Foreign key to User
        [Required]
        public int AuthorId { get; set; }

        public User? Author { get; set; }

        // Ectra
        //[Required]
        //public string Email { get; set; } = default!;

        //[Required]
        //public string Phone { get; set; } = default!;

        //[Required]
        //public DateTime? DateOfBirth { get; set; }

        //[Required]
        //public bool? IsActive { get; set; }

    }
}
