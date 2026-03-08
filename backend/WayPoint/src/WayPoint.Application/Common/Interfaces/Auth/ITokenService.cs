namespace WayPoint.Application.Common.Interfaces.Auth;

/// <summary>Application ใช้ primitive types เท่านั้น — ไม่ reference Infrastructure</summary>
public interface ITokenService
{
    Task<string> GenerateAccessTokenAsync(string userId, string fullName, string email, IList<string> roles, Guid? riderId);
    string GenerateRefreshToken();
}
