using Microsoft.AspNetCore.Mvc;
using TeduBlog.Core.SeedWorks;
using TeduBlog.WebApp.Models;

namespace TeduBlog.WebApp.Controllers
{
    public class SeriesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public SeriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [Route("/series")]
        public async Task<IActionResult> Index([FromQuery] int page = 1)
        {
            var series = await _unitOfWork.Series.GetAllPaging(string.Empty, page);
            return View(series);
        }

        [Route("series/{slug}")]
        public async Task<IActionResult> Details([FromRoute] string slug)
        {
            var posts =  await _unitOfWork.Series.GetAllPostsInSeries(slug);
            var series = await _unitOfWork.Series.GetBySlug(slug);
            return View(new SeriesDetailViewModel()
            {
                Posts = posts,
                Series = series
            });
        }
    }
}
