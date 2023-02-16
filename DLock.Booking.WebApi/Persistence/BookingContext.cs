using DLock.Booking.WebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLock.Booking.WebApi.Persistence
{
    public class BookingContext : DbContext
    {
        public BookingContext(DbContextOptions<BookingContext> options) : base(options)
        {
        }

        public BookingContext()
        {
        }

        public virtual DbSet<BookingEntity> Booking { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookingEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
        }
    }
}
