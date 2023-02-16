using DLock.Booking.WebApi.Entities;
using DLock.Booking.WebApi.Models;
using DLock.Booking.WebApi.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DLock.Booking.WebApi.Services
{
    public interface IBookingService
    {
        Task<bool> CheckIsFieldAvailableForBooking(string fieldName);
        Task<BookingModel> CreateBooking(CreateBookingModel model);
        Task<IEnumerable<BookingModel>> GetBookings(string userName = null);
    }

    public class BookingService : IBookingService
    {
        private readonly BookingContext _dbContext;
        private readonly IConfiguration _configuration;

        public BookingService(BookingContext dbContext,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<bool> CheckIsFieldAvailableForBooking(string fieldName)
        {
            return !await _dbContext.Booking.AnyAsync(b => b.FieldName == fieldName);
        }

        public async Task<BookingModel> CreateBooking(CreateBookingModel model)
        {
            await Task.Delay(_configuration.GetValue<int>("DelayTime")); // [IMPORTANT] simulate long running operation

            var booking = new BookingEntity
            {
                Id = Guid.NewGuid(),
                FieldName = model.FieldName,
                UserName = model.UserName,
                BookedTime = DateTimeOffset.UtcNow,
                BookedFromServerId = Program.ServerId
            };

            await _dbContext.AddAsync(booking);

            await _dbContext.SaveChangesAsync();

            return new BookingModel
            {
                Id = booking.Id,
                BookedTime = booking.BookedTime,
                FieldName = booking.FieldName,
                UserName = booking.UserName,
                BookedFromServerId = booking.BookedFromServerId,
            };
        }

        public async Task<IEnumerable<BookingModel>> GetBookings(string userName = null)
        {
            IQueryable<BookingEntity> query = _dbContext.Booking;

            if (!string.IsNullOrWhiteSpace(userName))
            {
                query = query.Where(b => b.UserName == userName);
            }

            var bookings = await query
                .OrderByDescending(b => b.BookedTime)
                .Select(b => new BookingModel
                {
                    Id = b.Id,
                    FieldName = b.FieldName,
                    UserName = b.UserName,
                    BookedTime = b.BookedTime,
                    BookedFromServerId = b.BookedFromServerId
                }).ToArrayAsync();

            return bookings;
        }
    }
}
