using Microsoft.AspNetCore.Mvc;
using TeduBlog.Core.SeedWorks;
using TeduBlog.WebApp.Models;

namespace TeduBlog.WebApp.Controllers
{
    public class PostController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PostController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Route("posts")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("posts/{categorySlug}")]
        public async Task<IActionResult> ListByCategory([FromRoute] string categorySlug, [FromQuery] int page = 1)
        {
            var posts = await _unitOfWork.Posts.GetPostByCategoryPaging(categorySlug, page, 2);
            var category = await _unitOfWork.PostCategories.GetBySlug(categorySlug);
            return View(new PostListByCategoryViewModel(){
                Posts = posts,
                Category = category
            });
        }

        [Route("tag/{tagSlug}")]
        public IActionResult ListByTag([FromRoute] string tagSlug, [FromQuery] int? page = 1)
        {
            return View();
        }

        [Route("post/{slug}")]
        public IActionResult Details([FromRoute] string slug)
        {
            return View();
        }
    }
}
