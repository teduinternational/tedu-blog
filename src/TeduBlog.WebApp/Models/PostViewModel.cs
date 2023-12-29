using TeduBlog.Core.Models;
using TeduBlog.Core.Models.Content;

namespace TeduBlog.WebApp.Models
{
    public class PostViewModel
    {
        public PagedResult<PostInListDto> Posts { get; set; }
    }

    public class SearchViewModel : PostViewModel
    {
        public string Keyword { get; set; }
    }
}
