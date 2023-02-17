using DLock.Identity.WebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLock.Identity.WebApi.Persistence
{
    public class IdentityContext : DbContext
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {
        }

        public IdentityContext()
        {
        }

        public virtual DbSet<UserEntity> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.HasKey(e => e.UserName);
            });
        }
    }
}
