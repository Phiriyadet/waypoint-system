using Microsoft.EntityFrameworkCore;

using WayPoint.Infrastructure.Data;

namespace WayPoint.Infrastructure.Identity;

public class RefreshTokenRepository
{
    private readonly ApplicationDbContext _context;

    public RefreshTokenRepository(ApplicationDbContext context) => _context = context;

    public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct = default)
    {
        return await _context.Set<RefreshToken>()
            .FirstOrDefaultAsync(rt => rt.Token == token && !rt.IsRevoked, ct);
    }

    public async Task AddAsync(RefreshToken refreshToken, CancellationToken ct = default)
    {
        await _context.Set<RefreshToken>().AddAsync(refreshToken, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task RevokeAsync(string token, string ipAddress, CancellationToken ct = default)
    {
        var refreshToken = await GetByTokenAsync(token, ct);
        if (refreshToken is null) return;
        refreshToken.IsRevoked = true;
        refreshToken.RevokedAt = DateTime.UtcNow;
        refreshToken.RevokedByIp = ipAddress;
        await _context.SaveChangesAsync(ct);
    }

    public async Task RevokeAllForUserAsync(string userId, CancellationToken ct = default)
    {
        var tokens = await _context.Set<RefreshToken>()
            .Where(rt => rt.UserId == userId && !rt.IsRevoked)
            .ToListAsync(ct);
        foreach (var token in tokens)
        {
            token.IsRevoked = true;
            token.RevokedAt = DateTime.UtcNow;
        }
        await _context.SaveChangesAsync(ct);
    }

    public async Task CleanupExpiredAsync(CancellationToken ct = default)
    {
        var expiredTokens = await _context.Set<RefreshToken>()
            .Where(rt => rt.ExpiresAt < DateTime.UtcNow)
            .ToListAsync(ct);
        _context.Set<RefreshToken>().RemoveRange(expiredTokens);
        await _context.SaveChangesAsync(ct);
    }
}
