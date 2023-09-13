using TeduBlog.Core.SeedWorks;

namespace TeduBlog.Data.SeedWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TeduBlogContext _context;

        public UnitOfWork(TeduBlogContext context)
        {
            _context = context;
        }
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
