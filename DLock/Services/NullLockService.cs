namespace DLock.Services
{
    public class NullLockService : ILockService
    {
        public const string Name = "No lock";
        public string ServiceName => Name;

        public Task<ILock> AcquireLock(string key)
        {
            return Task.FromResult<ILock>(new NullLock(key));
        }
    }

    class NullLock : ILock
    {
        public NullLock(string key)
        {
            Key = key;
        }

        public string Key { get; }

        public Task ReleaseLock()
        {
            return Task.CompletedTask;
        }
    }
}
