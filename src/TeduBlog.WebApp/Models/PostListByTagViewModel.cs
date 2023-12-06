using TeduBlog.Core.Models.Content;
using TeduBlog.Core.Models;

namespace TeduBlog.WebApp.Models
{
    public class PostListByTagViewModel
    {
        public TagDto Tag { get; set; }
        public PagedResult<PostInListDto> Posts { get; set; }
    }
}
