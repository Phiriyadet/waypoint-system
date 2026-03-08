using MediatR;
using WayPoint.Application.Common.Interfaces.Infrastructure;

namespace WayPoint.Application.Common.Behaviors;

public class CachingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, ICacheable
{
    private readonly ICacheService _cache;

    public CachingBehavior(ICacheService cache) => _cache = cache;

    public async Task<TResponse> Handle(
        TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var cached = await _cache.GetAsync<TResponse>(request.CacheKey, ct);
        if (cached is not null) return cached;

        var response = await next();

        await _cache.SetAsync(request.CacheKey, response, request.Expiry, ct);
        return response;
    }
}
