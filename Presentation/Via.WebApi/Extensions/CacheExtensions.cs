using Via.Domain.Enums;

namespace Via.WebApi.Extensions;

public static class CacheExtensions
{
    public static IServiceCollection AddCache(this IServiceCollection services, CacheType cacheType, IConfiguration configuration)
    {
        var connection = configuration.GetSection("CacheConnections");
        switch (cacheType)
        {
            case CacheType.InMemory:
               services.AddDistributedMemoryCache();
                break;
            case CacheType.Redis:
               services.AddStackExchangeRedisCache(opt => opt.Configuration = connection["SadıkDockerRedis"]);
                break;
            default:
                break;
        }




        return services;
    }
}
