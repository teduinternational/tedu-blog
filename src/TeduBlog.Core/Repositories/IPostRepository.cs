using TeduBlog.Core.Domain.Content;
using TeduBlog.Core.SeedWorks;

namespace TeduBlog.Core.Repositories
{
    public interface IPostRepository : IRepository<Post,Guid>
    {
        Task<List<Post>> GetPopularPostsAsync(int count);
    }
}
