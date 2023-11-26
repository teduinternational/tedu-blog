using TeduBlog.Core.Models.Content;
using TeduBlog.Core.Models;

namespace TeduBlog.WebApp.Models
{
    public class PostListByCategoryViewModel
    {
        public PostCategoryDto Category { get; set; }
        public PagedResult<PostInListDto> Posts { get; set; }
    }
}
