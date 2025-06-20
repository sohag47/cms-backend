using cms_backend.Data;
using cms_backend.Models;
using cms_backend.Models.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cms_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController(ApplicationDbContext _context) : ControllerBase
    {
        private readonly ApplicationDbContext context = _context;

        // GET: api/post
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var posts = await context.Posts.ToListAsync();
            return Ok(ApiResponse<IEnumerable<Post>>.Ok(posts, "Post found successfully"));
        }

        // GET api/post/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var post = await context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound(ApiResponse<string>.Fail("Post not found."));
            }

            return Ok(ApiResponse<Post>.Ok(post));
        }

        // POST api/post
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Post post)
        {
            context.Posts.Add(post);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = post.Id },
                ApiResponse<Post>.Ok(post, "Post created successfully."));
        }

        // PUT api/post/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Post updatedPost)
        {
            var post = await context.Posts.FindAsync(id);
            if (post == null)
                return NotFound(ApiResponse<string>.Fail("Post not found."));

            post.Title = updatedPost.Title;
            post.Slug = updatedPost.Slug;
            post.Content = updatedPost.Content;
            post.AuthorId = updatedPost.AuthorId;
            post.UpdatedBy = 1;
            post.UpdatedAt = DateTime.Now;

            await context.SaveChangesAsync();

            return Ok(ApiResponse<Post>.Ok(post, "Post updated successfully."));
        }

        // DELETE api/post/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
        {
            var post = await context.Posts.FindAsync(id);
            if (post == null) return NotFound(ApiResponse<string>.Fail("Post not found."));

            post.DeletedBy = 1;
            post.DeletedAt = DateTime.Now;
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
