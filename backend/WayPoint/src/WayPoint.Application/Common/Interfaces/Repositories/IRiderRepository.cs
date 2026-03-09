using WayPoint.Domain.Entities;
using WayPoint.Domain.Enums;

namespace WayPoint.Application.Common.Interfaces.Repositories;

public interface IRiderRepository
{
    Task<Rider?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<Rider>> GetByStatusAsync(RiderStatus status, CancellationToken ct = default);
    Task AddAsync(Rider rider, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
