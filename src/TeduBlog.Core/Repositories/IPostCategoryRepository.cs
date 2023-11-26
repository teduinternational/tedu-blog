using TeduBlog.Core.Domain.Content;
using TeduBlog.Core.Models.Content;
using TeduBlog.Core.Models;
using TeduBlog.Core.SeedWorks;

namespace TeduBlog.Core.Repositories
{
    public interface IPostCategoryRepository : IRepository<PostCategory, Guid>
    {
        Task<PagedResult<PostCategoryDto>> GetAllPaging(string? keyword, int pageIndex = 1, int pageSize = 10);
        Task<bool> HasPost(Guid categoryId);

        Task<PostCategoryDto> GetBySlug(string slug);

    }
}
