using cms_backend.Data;
using cms_backend.Models;

namespace cms_backend.Repositories
{
    public class PostRepository(ApplicationDbContext context) : Repository<Post>(context), IPostRepository
    {
    }
}
