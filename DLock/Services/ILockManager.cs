using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace DLock.Services
{
    public interface ILockManager
    {
        ILockService GetLockService();
        void SetLockService(string serviceName);
    }

    public class LockManager : ILockManager
    {
        const string LockServiceKey = "dlock-lockservice";

        private readonly IConfiguration _configuration;
        private readonly ConnectionMultiplexer _multiplexer;
        private readonly NullLockService _nullLockService;
        private readonly LocalLockService _localLockService;
        private readonly RedLockService _redLockService;
        private readonly SingleRedisLockService _redisLockService;

        public LockManager(IConfiguration configuration,
            ConnectionMultiplexer multiplexer,
            RedLockService redLockService,
            SingleRedisLockService redisLockService)
        {
            _configuration = configuration;
            _multiplexer = multiplexer;
            _nullLockService = new NullLockService();
            _localLockService = new LocalLockService(configuration);
            _redLockService = redLockService;
            _redisLockService = redisLockService;
        }

        public ILockService GetLockService()
        {
            var db = _multiplexer.GetDatabase();
            var serviceName = db.StringGet(LockServiceKey);

            switch (serviceName)
            {
                case LocalLockService.Name:
                    {
                        return _localLockService;
                    }
                case SingleRedisLockService.Name:
                    {
                        return _redisLockService;
                    }
                case RedLockService.Name:
                    {
                        return _redLockService;
                    }
                default:
                    {
                        return _nullLockService;
                    }
            }
        }

        public void SetLockService(string serviceName)
        {
            var db = _multiplexer.GetDatabase();

            db.StringSet(LockServiceKey, serviceName);
        }
    }
}
