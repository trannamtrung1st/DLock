using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DLock.Identity.WebApi.Persistence
{
    public class IdentityContextFactory : IDesignTimeDbContextFactory<IdentityContext>
    {
        public IdentityContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<IdentityContext>()
                .UseSqlServer();

            return new IdentityContext(builder.Options);
        }
    }
}
