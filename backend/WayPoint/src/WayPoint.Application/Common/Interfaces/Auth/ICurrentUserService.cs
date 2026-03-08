namespace WayPoint.Application.Common.Interfaces.Auth;

public interface ICurrentUserService
{
    string UserId { get; }
    string Role { get; }
    Guid? RiderId { get; }  // null ถ้าเป็น Admin
    bool IsAdmin { get; }
    bool IsRider { get; }
}
