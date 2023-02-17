namespace DLock.Services
{
    public interface ILock
    {
        public string Key { get; }
        Task ReleaseLock();
    }
}
