using System.Security.Claims;

using Microsoft.AspNetCore.Http;

using WayPoint.Application.Common.Interfaces.Auth;

namespace WayPoint.Infrastructure.Identity;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        => _httpContextAccessor = httpContextAccessor;

    public string UserId
        => _httpContextAccessor.HttpContext?.User?
            .FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException();

    public string Role
        => _httpContextAccessor.HttpContext?.User?
            .FindFirstValue(ClaimTypes.Role)
        ?? "Guest";

    public Guid? RiderId
    {
        get
        {
            var riderIdClaim = _httpContextAccessor.HttpContext?.User?
                .FindFirstValue("riderId");
            return string.IsNullOrEmpty(riderIdClaim)
                ? null
                : Guid.Parse(riderIdClaim);
        }
    }

    public bool IsAdmin => Role == "Admin";
    public bool IsRider => Role == "Rider";
}
