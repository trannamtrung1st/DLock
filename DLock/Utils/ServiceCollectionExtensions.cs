using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DLock.Utils
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRedisConnection(this IServiceCollection services,
            IConfiguration configuration)
        {
            return services.AddSingleton(provider =>
            {
                var endpoints = configuration.GetSection("RedisEndpoints").Get<IEnumerable<string>>();
                return RedisHelper.GetConnectionMultiplexer(endpoints.First());
            });
        }
    }
}
