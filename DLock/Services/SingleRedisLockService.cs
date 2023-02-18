using DLock.Utils;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace DLock.Services
{
    public class SingleRedisLockService : ILockService, IDisposable
    {
        private readonly IConfiguration _configuration;
        private ConnectionMultiplexer _multiplexer;

        public SingleRedisLockService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public const string Name = "Redis lock";
        public string ServiceName => Name;

        public async Task<ILock> AcquireLock(string key)
        {
            Initialize();

            var db = _multiplexer.GetDatabase();

            // Equivalent command: SET resource_name a_random_value NX PX {Milliseconds}
            string finalKey = $"redis-lock-{key}";
            string randomVal = Guid.NewGuid().ToString();

            bool acquired = await db.StringSetAsync(key: finalKey, value: randomVal,
                expiry: TimeSpan.FromMilliseconds(_configuration.GetValue<int>("LockExpiryMs")),
                when: When.NotExists);

            return acquired ? new SingleRedisLock(finalKey, randomVal, _multiplexer) : null;
        }

        private void Initialize()
        {
            if (_multiplexer == null)
            {
                var endpoints = _configuration.GetSection("RedisEndpoints").Get<IEnumerable<string>>();
                _multiplexer = RedisHelper.GetConnectionMultiplexer(endpoints.First());
            }
        }

        public void Dispose()
        {
            if (_multiplexer != null)
            {
                _multiplexer.Dispose();
            }
        }
    }

    class SingleRedisLock : ILock
    {
        private readonly ConnectionMultiplexer _multiplexer;
        private readonly string _randomVal;

        public SingleRedisLock(string key, string randomVal, ConnectionMultiplexer multiplexer)
        {
            Key = key;
            _randomVal = randomVal;
            _multiplexer = multiplexer;
        }

        public string Key { get; }

        public async Task ReleaseLock()
        {
            await _multiplexer.GetDatabase().ScriptEvaluateAsync(LuaScript.Prepare($@"
if redis.call(""get"",@key) == @value then
    return redis.call(""del"", @key)
else
    return 0
end
"), parameters: new { key = Key, value = _randomVal });
        }
    }
}
