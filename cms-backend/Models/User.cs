using cms_backend.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace cms_backend.Models
{
    public class User : AuditableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(50, MinimumLength = 3)]
        public string Username { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required, StringLength(100, MinimumLength = 6)]
        public string PasswordHash { get; set; } = null!;

        public ICollection<Post>? Posts { get; set; }
    }
}
