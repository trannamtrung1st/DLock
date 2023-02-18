using Microsoft.Extensions.Configuration;

namespace DLock.Services
{
    public interface ILockManager
    {
        ILockService GetLockService();
        void SetLockService(string serviceName);
        void Reset();
    }

    public class LockManager : ILockManager
    {
        private ILockService _lockService;
        private readonly IConfiguration _configuration;
        private readonly RedLockService _redLockService;
        private readonly SingleRedisLockService _redisLockService;

        public LockManager(IConfiguration configuration,
            RedLockService redLockService,
            SingleRedisLockService redisLockService)
        {
            _configuration = configuration;
            _redLockService = redLockService;
            _redisLockService = redisLockService;

            Reset();
        }

        public ILockService GetLockService()
        {
            return _lockService;
        }

        public void SetLockService(string serviceName)
        {
            switch (serviceName)
            {
                case NullLockService.Name:
                    {
                        _lockService = new NullLockService();
                        break;
                    }
                case LocalLockService.Name:
                    {
                        _lockService = new LocalLockService(_configuration);
                        break;
                    }
                case SingleRedisLockService.Name:
                    {
                        _lockService = _redisLockService;
                        break;
                    }
                case RedLockService.Name:
                    {
                        _lockService = _redLockService;
                        break;
                    }
            }
        }

        public void Reset()
        {
            _lockService = new NullLockService();
        }
    }
}
