using Microsoft.EntityFrameworkCore;

using WayPoint.Application.Common.Interfaces.Repositories;
using WayPoint.Domain.Entities;
using WayPoint.Domain.Enums;

namespace WayPoint.Infrastructure.Data.Repositories;

public class RiderRepository : IRiderRepository
{
    private readonly ApplicationDbContext _context;

    public RiderRepository(ApplicationDbContext context) => _context = context;

    public async Task<Rider?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Riders.FindAsync([id], ct);

    public async Task<List<Rider>> GetByStatusAsync(
        RiderStatus status, CancellationToken ct = default)
    {
        return await _context.Riders
            .Where(r => r.Status == status)
            .OrderBy(r => r.Name)
            .ToListAsync(ct);
    }

    public async Task AddAsync(Rider rider, CancellationToken ct = default)
        => await _context.Riders.AddAsync(rider, ct);

    public async Task SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);
}
