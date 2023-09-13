namespace TeduBlog.Core.SeedWorks
{
    public interface IUnitOfWork
    {
        Task<int> CompleteAsync();
    }
}
