using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeduBlog.Data
{
    public class TeduBlogContextFactory : IDesignTimeDbContextFactory<TeduBlogContext>
    {
        public TeduBlogContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .Build();
            var builder = new DbContextOptionsBuilder<TeduBlogContext>();
            builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            return new TeduBlogContext(builder.Options);
        }
    }
}
