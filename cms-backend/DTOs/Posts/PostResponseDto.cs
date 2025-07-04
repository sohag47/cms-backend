﻿using cms_backend.DTOs.Users;
using cms_backend.Models;

namespace cms_backend.DTOs.Posts
{
    public class PostResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string Content { get; set; } = null!;
        public int AuthorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public UserResponseDto? Author { get; set; }
    }
}
