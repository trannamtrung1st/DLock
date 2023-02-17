namespace DLock.Booking.WebApi.Entities
{
    public class BookingEntity
    {
        public BookingEntity()
        {
        }

        public Guid Id { get; set; }
        public string FieldName { get; set; }
        public string UserName { get; set; }
        public DateTimeOffset BookedTime { get; set; }
        public string BookedFromServerId { get; set; }
    }
}
