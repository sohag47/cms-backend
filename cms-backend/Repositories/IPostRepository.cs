using cms_backend.DTOs.Posts;
using cms_backend.Models;
using System.Linq.Expressions;

namespace cms_backend.Repositories
{
    public interface IPostRepository : IRepository<Post> 
    {
        Task<PostResponseDto> CreatePostAsync(PostCreateDto dto);
    }
}
