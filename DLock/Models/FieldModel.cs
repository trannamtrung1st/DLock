namespace DLock.Models
{
    public class FieldModel
    {
        public string Name { get; set; }
        public bool IsUnderMaintenance { get; set; }
        public DateTimeOffset? MaintenanceTime { get; set; }
    }
}
