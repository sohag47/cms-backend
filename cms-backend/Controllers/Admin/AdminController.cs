using Microsoft.AspNetCore.Mvc;

namespace cms_backend.Controllers.Admin;

[Route("Admin")]
public class AdminController : Controller
{
    [HttpGet("")]
    public IActionResult Index()
    {
        return View();
    }
}
