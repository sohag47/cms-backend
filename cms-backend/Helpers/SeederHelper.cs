using cms_backend.Models;
using Microsoft.AspNetCore.Identity;

namespace cms_backend.Helpers
{
    public static class SeederHelper
    {
        public static string HashPassword(string plainPassword)
        {
            var hasher = new PasswordHasher<User>();
            return hasher.HashPassword(new User(), plainPassword);
        }
    }
}
