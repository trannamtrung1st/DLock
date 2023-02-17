namespace DLock.Field.WebApi.Entities
{
    public class FieldEntity
    {
        public FieldEntity()
        {
        }

        public FieldEntity(string name)
        {
            Name = name;
            IsUnderMaintenance = false;
            MaintenanceTime = null;
        }

        public string Name { get; set; }
        public bool IsUnderMaintenance { get; set; }
        public DateTimeOffset? MaintenanceTime { get; set; }
    }
}
