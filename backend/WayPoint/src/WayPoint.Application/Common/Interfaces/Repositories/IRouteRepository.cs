using WayPoint.Domain.Entities;

namespace WayPoint.Application.Common.Interfaces.Repositories;

public interface IRouteRepository
{
    Task<Route?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(Route route, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
