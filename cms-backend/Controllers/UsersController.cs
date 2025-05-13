using cms_backend.Data;
using cms_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace cms_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public UsersController(ApplicationDbContext _context)
        {
            context = _context;
        }
        // GET: api/<UsersController>
        [HttpGet]
        public ActionResult<ApiResponse<IEnumerable<User>>> Get()
        {
            var users = context.Users.ToList();
            return Ok(new ApiResponse<IEnumerable<User>>(users, "Users Found Succefully"));
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public ActionResult<ApiResponse<User>> Get(int id)
        {
            var user = context.Users.Find(id);
            if (user == null) return NotFound(new ApiResponse<User>(null, "User not found", false));
            return Ok(new ApiResponse<User>(user));
        }

        // POST api/<UsersController>
        [HttpPost]
        public ActionResult<ApiResponse<User>> Post([FromBody] User user)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse<User>(null, "Invalid data", false));

            context.Users.Add(user);
            context.SaveChanges();
            return CreatedAtAction(nameof(Get), new { id = user.Id }, new ApiResponse<User>(user, "User created successfully"));

        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public ActionResult<ApiResponse<User>> Put(int id, [FromBody] User user)
        {
            if (id != user.Id) return BadRequest(new ApiResponse<User>(null, "Invalid data", false));
            context.Entry(user).State = EntityState.Modified;
            context.SaveChanges();
            return Ok(new ApiResponse<User>(user, "User updated"));
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public ActionResult<ApiResponse<string>> Delete(int id)
        {
            var user = context.Users.Find(id);
            if (user == null) return NotFound(new ApiResponse<string>(null, "User not found", false));

            context.Users.Remove(user);
            context.SaveChanges();

            return Ok(new ApiResponse<string>("User deleted successfully"));
        }
    }
}
