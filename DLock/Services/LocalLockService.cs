using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;

namespace DLock.Services
{
    public class LocalLockService : ILockService
    {
        private readonly ConcurrentDictionary<string, LocalLock> _lockMap;
        private readonly IConfiguration _configuration;

        public LocalLockService(IConfiguration configuration)
        {
            _lockMap = new ConcurrentDictionary<string, LocalLock>();
            _configuration = configuration;
        }

        public const string Name = "Local lock";
        public string ServiceName => Name;

        public async Task<ILock> AcquireLock(string key)
        {
            var lockObj = _lockMap.GetOrAdd(key, (key) => new LocalLock(key, () =>
            {
                if (_lockMap.TryRemove(key, out var lockObj))
                {
                    lockObj.Release();
                }

                return Task.CompletedTask;
            }));

            var acquired = await lockObj.WaitAsync(_configuration.GetValue<int>("LockWaitTimeMs"));

            return acquired ? lockObj : null;
        }
    }

    class LocalLock : SemaphoreSlim, ILock
    {
        private readonly Func<Task> _releaseAction;

        public LocalLock(string key, Func<Task> releaseAction) : base(1, 1)
        {
            Key = key;
            _releaseAction = releaseAction;
        }

        public string Key { get; }

        public Task ReleaseLock() => _releaseAction();
    }
}
