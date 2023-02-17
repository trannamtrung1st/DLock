using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DLock.Booking.WebApi.Persistence
{
    public class BookingContextFactory : IDesignTimeDbContextFactory<BookingContext>
    {
        public BookingContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<BookingContext>()
                .UseSqlServer();

            return new BookingContext(builder.Options);
        }
    }
}
