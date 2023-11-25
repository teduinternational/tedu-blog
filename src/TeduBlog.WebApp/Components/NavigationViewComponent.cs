using Microsoft.AspNetCore.Mvc;

namespace TeduBlog.WebApp.Components
{
    public class NavigationViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
