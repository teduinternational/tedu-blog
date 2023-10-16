using TeduBlog.Core.SeedWorks;
using TeduBlog.Core.Domain.Identity;

namespace TeduBlog.Core.Repositories
{
    public interface IUserRepository : IRepository<AppUser, Guid>
    {
        Task RemoveUserFromRoles(Guid userId, string[] roles);
    }
}
