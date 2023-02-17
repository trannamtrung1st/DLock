namespace DLock.Services
{
    public interface ILockService
    {
        string ServiceName { get; }

        Task<ILock> AcquireLock(string key);
    }
}
