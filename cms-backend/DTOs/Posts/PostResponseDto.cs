namespace cms_backend.DTOs.Posts
{
    public class PostResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string AuthorUsername { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
