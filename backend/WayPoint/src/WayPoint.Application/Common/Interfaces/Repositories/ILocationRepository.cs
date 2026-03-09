using NetTopologySuite.Geometries;
using Location = WayPoint.Domain.Entities.Location;

namespace WayPoint.Application.Common.Interfaces.Repositories;

public interface ILocationRepository
{
    Task<Location?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Location?> FindNearbyAsync(Point coordinate, double radiusMeters, CancellationToken ct = default);
    Task<List<Location>> GetNearbyAsync(Point coordinate, double radiusMeters, int limit, CancellationToken ct = default);
    Task<List<Location>> SearchAsync(string query, int limit, CancellationToken ct = default);
    Task AddAsync(Location location, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
