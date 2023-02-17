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
        private readonly DistributedLockService _distributedLockService;

        public LockManager(IConfiguration configuration,
            DistributedLockService distributedLockService)
        {
            _configuration = configuration;
            _distributedLockService = distributedLockService;

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
                case DistributedLockService.Name:
                    {
                        _lockService = _distributedLockService;
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
