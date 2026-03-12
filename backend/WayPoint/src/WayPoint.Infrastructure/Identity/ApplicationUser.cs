using Microsoft.AspNetCore.Identity;

namespace WayPoint.Infrastructure.Identity;

/// <summary>
/// Extends IdentityUser with custom properties
/// </summary>
public class ApplicationUser : IdentityUser
{
    /// <summary>FK to Rider entity (null สำหรับ Admin)</summary>
    public Guid? RiderId { get; set; }

    /// <summary>Full name of the user</summary>
    public string FullName { get; set; } = string.Empty;
}
