namespace DLock.Booking.WebApi.Models
{
    public class BookingModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FieldName { get; set; }
        public DateTimeOffset BookedTime { get; set; }
        public string BookedFromServerId { get; set; }
    }
}
