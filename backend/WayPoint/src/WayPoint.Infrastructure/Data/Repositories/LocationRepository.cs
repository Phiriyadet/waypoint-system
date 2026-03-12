using Microsoft.EntityFrameworkCore;

using NetTopologySuite.Geometries;

using WayPoint.Application.Common.Interfaces.Repositories;

using Location = WayPoint.Domain.Entities.Location;

namespace WayPoint.Infrastructure.Data.Repositories;

public class LocationRepository : ILocationRepository
{
    private readonly ApplicationDbContext _context;

    public LocationRepository(ApplicationDbContext context) => _context = context;

    public async Task<Location?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Locations.FindAsync([id], ct);

    public async Task<Location?> FindNearbyAsync(
        Point coordinate, double radiusMeters, CancellationToken ct = default)
    {
        return await _context.Locations
            .Where(l => l.Coordinate.Distance(coordinate) <= radiusMeters)
            .OrderBy(l => l.Coordinate.Distance(coordinate))
            .FirstOrDefaultAsync(ct);
    }

    public async Task<List<Location>> GetNearbyAsync(
        Point coordinate, double radiusMeters, int limit, CancellationToken ct = default)
    {
        return await _context.Locations
            .Where(l => l.Coordinate.Distance(coordinate) <= radiusMeters)
            .OrderBy(l => l.Coordinate.Distance(coordinate))
            .Take(limit)
            .ToListAsync(ct);
    }

    public async Task<List<Location>> SearchAsync(
        string query, int limit, CancellationToken ct = default)
    {
        return await _context.Locations
            .Where(l => EF.Functions.ILike(l.Address.AddressInfo, $"%{query}%")
                     || EF.Functions.ILike(l.PlaceName!, $"%{query}%"))
            .OrderByDescending(l => l.ConfidenceScore.Value)
            .Take(limit)
            .ToListAsync(ct);
    }

    public async Task AddAsync(Location location, CancellationToken ct = default)
        => await _context.Locations.AddAsync(location, ct);

    public async Task SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);
}
