using AutoMapper;
using cms_backend.Data;
using cms_backend.DTOs.Posts;
using cms_backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace cms_backend.Repositories
{
    public class PostRepository(ApplicationDbContext context, IMapper _mapper) : Repository<Post>(context), IPostRepository
    {
        private readonly IMapper mapper = _mapper;



        public async Task<PostResponseDto> CreatePostAsync(PostCreateDto dto)
        {
            var post = mapper.Map<Post>(dto);
            await AddAsync(post);
            await SaveChangesAsync();

            var responseDto = mapper.Map<PostResponseDto>(post);
            return responseDto;
        }

    }
}
