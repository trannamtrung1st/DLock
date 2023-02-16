using DLock.Field.WebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLock.Field.WebApi.Persistence
{
    public class FieldContext : DbContext
    {
        public FieldContext(DbContextOptions<FieldContext> options) : base(options)
        {
        }

        public FieldContext()
        {
        }

        public virtual DbSet<FieldEntity> Field { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FieldEntity>(entity =>
            {
                entity.HasKey(e => e.Name);

                entity.HasData(Enumerable.Range(1, 18).Select(i => new FieldEntity($"Field {i:00}")));
            });
        }
    }
}
