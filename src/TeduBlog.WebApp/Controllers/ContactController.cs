using Microsoft.AspNetCore.Mvc;

namespace TeduBlog.WebApp.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
