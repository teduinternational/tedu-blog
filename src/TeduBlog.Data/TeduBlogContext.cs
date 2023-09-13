using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TeduBlog.Core.Domain.Content;
using TeduBlog.Core.Domain.Identity;

namespace TeduBlog.Data
{
    public class TeduBlogContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public TeduBlogContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostCategory> PostCategories { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PostActivityLog> PostActivityLogs { get; set; }
        public DbSet<Series> Series { get; set; }
        public DbSet<PostInSeries> PostInSeries { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims").HasKey(x => x.Id);

            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims")
            .HasKey(x => x.Id);

            builder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);

            builder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles")
            .HasKey(x => new { x.RoleId, x.UserId });

            builder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens")
               .HasKey(x => new { x.UserId });
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.State == EntityState.Added || e.State ==  EntityState.Modified);

            foreach (var entityEntry in entries)
            {
                var dateCreatedProp = entityEntry.Entity.GetType().GetProperty("DateCreated");
                if (entityEntry.State == EntityState.Added
                    && dateCreatedProp != null)
                {
                    dateCreatedProp.SetValue(entityEntry.Entity, DateTime.Now);
                }
                var modifiedDateProp = entityEntry.Entity.GetType().GetProperty("ModifiedDate");
                if (entityEntry.State == EntityState.Modified
                    && modifiedDateProp != null)
                {
                    modifiedDateProp.SetValue(entityEntry.Entity, DateTime.Now);
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
