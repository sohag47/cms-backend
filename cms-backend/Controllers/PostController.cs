using AutoMapper;
using cms_backend.Data;
using cms_backend.DTOs.Posts;
using cms_backend.Models;
using cms_backend.Models.Base;
using cms_backend.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace cms_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController(IPostRepository _postRepo, IMapper _mapper) : ControllerBase
    {
        private readonly IPostRepository postRepo = _postRepo;
        private readonly IMapper mapper = _mapper;

        // GET: api/post
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var posts = await postRepo.GetAllAsync(p => p.Author);
            var postDtos = mapper.Map<IEnumerable<PostResponseDto>>(posts);
            return Ok(ApiResponse<IEnumerable<PostResponseDto>>.Ok(postDtos));
        }

        // GET api/post/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Show(int id)
        {
            var post = await postRepo.GetByIdAsync(id, p => p.Author);

            if (post == null)
                return NotFound(ApiResponse<string>.Fail("Post not found."));

            var postDto = mapper.Map<PostResponseDto>(post);
            return Ok(ApiResponse<PostResponseDto>.Ok(postDto, "Post found successfully"));
        }

        // POST api/post
        [HttpPost]
        public async Task<IActionResult> Store([FromBody] PostCreateDto dto)
        {
            var responseDto = await postRepo.CreatePostAsync(dto);

            return CreatedAtAction(nameof(Show), new { id = responseDto.Id },
                ApiResponse<PostResponseDto>.Ok(responseDto, "Post created successfully."));
        }

        // PUT api/post/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Post updatedPost)
        {
            var post = await postRepo.GetByIdAsync(id);
            if (post == null)
                return NotFound(ApiResponse<string>.Fail("Post not found."));

            post.Title = updatedPost.Title;
            post.Slug = updatedPost.Slug;
            post.Content = updatedPost.Content;
            post.AuthorId = 1;

            postRepo.Update(post);
            await postRepo.SaveChangesAsync();

            return CreatedAtAction(nameof(Show), new { id = post.Id },
                ApiResponse<Post>.Ok(post, "Post updated successfully."));
        }

        // DELETE api/post/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await postRepo.GetByIdAsync(id);
            if (post == null) return NotFound(ApiResponse<string>.Fail("Post not found."));

            postRepo.Delete(post);
            await postRepo.SaveChangesAsync();
            return Ok(ApiResponse<string>.Ok(null, "Post deleted successfully."));
        }
    }
}
