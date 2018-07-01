using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Oboete.Database
{
    public class OboeteContextFactory : IDesignTimeDbContextFactory<OboeteContext>
    {
        public OboeteContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<OboeteContext>();

            builder.UseSqlServer(@"Server=LocalHost\SQLEXPRESS;Database=OboeteDB;Trusted_Connection=True;MultipleActiveResultSets=True");

            return new OboeteContext(builder.Options);
        }
    }
}