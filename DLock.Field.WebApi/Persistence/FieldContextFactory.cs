using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DLock.Field.WebApi.Persistence
{
    public class FieldContextFactory : IDesignTimeDbContextFactory<FieldContext>
    {
        public FieldContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<FieldContext>()
                .UseSqlServer();

            return new FieldContext(builder.Options);
        }
    }
}
