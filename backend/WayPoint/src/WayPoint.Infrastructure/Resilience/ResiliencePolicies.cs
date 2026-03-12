using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;
using Polly.Fallback;
using Polly.Timeout;

using WayPoint.Application.Common.Interfaces.ExternalServices;

namespace WayPoint.Infrastructure.Resilience;

public static class ResiliencePolicies
{
    /// <summary>
    /// Exponential backoff retry policy for HTTP requests
    /// Retries 3 times: 2s, 4s, 8s
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError() // 5xx and 408
            .Or<TimeoutRejectedException>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    // Log retry attempt
                    var msg = outcome.Exception?.Message ?? outcome.Result.StatusCode.ToString();
                    Console.WriteLine(
                        $"Retry {retryCount} after {timespan.TotalSeconds}s due to: {msg}");
                });
    }

    /// <summary>
    /// Circuit breaker policy
    /// Opens after 5 consecutive failures, stays open for 1 minute
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromMinutes(1),
                onBreak: (outcome, duration) =>
                {
                    var msg = outcome.Exception?.Message ?? outcome.Result.StatusCode.ToString();
                    Console.WriteLine(
                        $"Circuit breaker opened for {duration.TotalSeconds}s due to: {msg}");
                },
                onReset: () => Console.WriteLine("Circuit breaker reset"),
                onHalfOpen: () => Console.WriteLine("Circuit breaker half-open"));
    }

    /// <summary>
    /// Timeout policy - limit operation to 30 seconds
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy()
    {
        return Policy
            .TimeoutAsync<HttpResponseMessage>(
                timeout: TimeSpan.FromSeconds(30),
                onTimeoutAsync: (context, timespan, task) =>
                {
                    Console.WriteLine($"Request timed out after {timespan.TotalSeconds}s");
                    return Task.CompletedTask;
                });
    }

    /// <summary>
    /// Combined policy: Timeout → Retry → Circuit Breaker
    /// Order matters! Inner policies execute first
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy()
    {
        return Policy.WrapAsync(
            GetCircuitBreakerPolicy(),  // Outer: Circuit Breaker
            GetRetryPolicy(),           // Middle: Retry
            GetTimeoutPolicy()          // Inner: Timeout
        );
    }

    /// <summary>
    /// Fallback policy for routing service
    /// Falls back from ORS to OSRM
    /// </summary>
    public static AsyncFallbackPolicy<RouteResult> GetRoutingFallbackPolicy(
        Func<CancellationToken, Task<RouteResult>> fallbackAction)
    {
        return Policy<RouteResult>
            .Handle<HttpRequestException>()
            .Or<BrokenCircuitException>()
            .Or<TimeoutRejectedException>()
            .FallbackAsync(
                fallbackAction: (ctx, token) => fallbackAction(token),
                onFallbackAsync: (result, context) =>
                {
                    Console.WriteLine("Falling back to OSRM routing service");
                    return Task.CompletedTask;
                });
    }
}
