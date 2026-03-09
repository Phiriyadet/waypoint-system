using WayPoint.Domain.Entities;
using WayPoint.Domain.Enums;

namespace WayPoint.Application.Common.Interfaces.Repositories;

public interface IDeliveryRepository
{
    Task<Delivery?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<Delivery>> GetByRiderIdAsync(Guid riderId, CancellationToken ct = default);
    Task<List<Delivery>> GetAllAsync(Guid? riderId, DeliveryStatus? status, CancellationToken ct = default);
    Task AddAsync(Delivery delivery, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
