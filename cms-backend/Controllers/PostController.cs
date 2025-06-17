using cms_backend.Data;
using cms_backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cms_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public PostController(ApplicationDbContext _context)
        {
            context = _context;
        }

        // GET: api/<PostController>
        [HttpGet]
        public ActionResult<ApiResponse<IEnumerable<Post>>> Get()
        {
            var posts = context.Posts.ToList();
            return Ok(new ApiResponse<IEnumerable<Post>>(posts, "posts Found Succefully"));
        }

        // GET api/<PostController>/5
        [HttpGet("{id}")]
        public ActionResult<ApiResponse<Post>> Get(int id)
        {
            var post = context.Posts.Find(id);
            if (post == null) return NotFound(new ApiResponse<Post>(null, "Post not found", false));
            return Ok(new ApiResponse<Post>(post));
        }

        // POST api/<PostController>
        [HttpPost]
        public ActionResult<ApiResponse<Post>> Post([FromBody] Post post)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse<Post>(null, "Invalid data", false));

            context.Posts.Add(post);
            context.SaveChanges();
            return CreatedAtAction(nameof(Get), new { id = post.Id }, new ApiResponse<Post>(post, "Post created successfully"));

        }

        // PUT api/<PostController>/5
        [HttpPut("{id}")]
        public ActionResult<ApiResponse<Post>> Put(int id, [FromBody] Post post)
        {
            if (id != post.Id) return BadRequest(new ApiResponse<Post>(null, "Invalid data", false));
            context.Entry(post).State = EntityState.Modified;
            context.SaveChanges();
            return Ok(new ApiResponse<Post>(post, "Post updated"));
        }

        // DELETE api/<PostController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
        {
            var post = await context.Posts.FindAsync(id);
            if (post == null) return NotFound(new ApiResponse<string>(null, "Post not found", false));

            post.IsDeleted = true;
            await context.SaveChangesAsync();

            return Ok(new ApiResponse<string>("Post deleted successfully"));
        }
        //public ActionResult<ApiResponse<string>> Delete(int id)
        //{
        //    var post = context.Posts.Find(id);
        //    if (post == null) return NotFound(new ApiResponse<string>(null, "Post not found", false));

        //    post.IsDeleted = true;

        //    //context.Posts.Remove(post);
        //    //context.SaveChanges();

        //    return Ok(new ApiResponse<string>("Post deleted successfully"));
        //}
    }
}
