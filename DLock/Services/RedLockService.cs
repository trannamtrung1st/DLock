using DLock.Utils;
using Microsoft.Extensions.Configuration;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;

namespace DLock.Services
{
    public class RedLockService : ILockService, IDisposable
    {
        private readonly IConfiguration _configuration;
        private RedLockFactory _redLockFactory;
        private IEnumerable<ConnectionMultiplexer> _multiplexers;

        public RedLockService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public const string Name = "Red lock";
        public string ServiceName => Name;

        public async Task<ILock> AcquireLock(string key)
        {
            var factory = Initialize();

            var redLock = await factory.CreateLockAsync(key,
                expiryTime: TimeSpan.FromMilliseconds(_configuration.GetValue<int>("LockExpiryMs")),
                waitTime: TimeSpan.FromMilliseconds(_configuration.GetValue<int>("LockWaitTimeMs")),
                retryTime: TimeSpan.FromMilliseconds(_configuration.GetValue<int>("LockRetryMs")));

            return redLock.IsAcquired ? new DistributedLock(redLock) : null;
        }

        public async Task ResetData()
        {
            Initialize();

            foreach (var server in _multiplexers.SelectMany(m => m.GetServers()).ToArray())
            {
                await server.FlushAllDatabasesAsync();
            }
        }

        private RedLockFactory Initialize()
        {
            if (_redLockFactory == null)
            {
                var endpoints = _configuration.GetSection("RedisEndpoints").Get<IEnumerable<string>>();
                _multiplexers = endpoints.Select(e => RedisHelper.GetConnectionMultiplexer(e)).ToArray();
                var redLockMultiplexers = new List<RedLockMultiplexer>(_multiplexers.Select(m => (RedLockMultiplexer)m).ToArray());
                _redLockFactory = RedLockFactory.Create(redLockMultiplexers);
            }

            return _redLockFactory;
        }

        public void Dispose()
        {
            if (_redLockFactory != null) _redLockFactory.Dispose();

            if (_multiplexers != null)
            {
                foreach (var multiplexer in _multiplexers)
                {
                    multiplexer.Dispose();
                }
            }
        }
    }

    class DistributedLock : ILock, IDisposable
    {
        private readonly IRedLock _redLock;

        public DistributedLock(IRedLock redLock)
        {
            _redLock = redLock;
        }

        public string Key => _redLock.Resource;

        public void Dispose()
        {
            _redLock.Dispose();
        }

        public async Task ReleaseLock()
        {
            await _redLock.DisposeAsync();
        }
    }
}
