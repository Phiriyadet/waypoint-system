using System.Text.Json;

using Microsoft.Extensions.Caching.Distributed;

using WayPoint.Application.Common.Interfaces.Infrastructure;

namespace WayPoint.Infrastructure.Services.Caching;

public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _cache;

    public RedisCacheService(IDistributedCache cache) => _cache = cache;

    public async Task<T?> GetAsync<T>(string key, CancellationToken ct = default)
    {
        var bytes = await _cache.GetAsync(key, ct);
        return bytes is null
            ? default
            : JsonSerializer.Deserialize<T>(bytes);
    }

    public async Task SetAsync<T>(
        string key, T value, TimeSpan? expiry = null, CancellationToken ct = default)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(value);
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromMinutes(5)
        };
        await _cache.SetAsync(key, bytes, options, ct);
    }

    public async Task RemoveAsync(string key, CancellationToken ct = default)
        => await _cache.RemoveAsync(key, ct);

    public async Task RemoveByPrefixAsync(string prefix, CancellationToken ct = default)
    {
        // Note: Redis prefix removal requires direct Redis connection
        // This is a simplified version - production should use StackExchange.Redis directly
        await Task.CompletedTask;
    }
}
