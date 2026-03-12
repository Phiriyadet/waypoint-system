using Microsoft.EntityFrameworkCore;

using WayPoint.Application.Common.Interfaces.Repositories;
using WayPoint.Domain.Entities;
using WayPoint.Domain.Enums;

namespace WayPoint.Infrastructure.Data.Repositories;

public class DeliveryRepository : IDeliveryRepository
{
    private readonly ApplicationDbContext _context;

    public DeliveryRepository(ApplicationDbContext context) => _context = context;

    public async Task<Delivery?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Deliveries
            .Include(d => d.Logs)
            .FirstOrDefaultAsync(d => d.Id == id, ct);

    public async Task<List<Delivery>> GetByRiderIdAsync(
        Guid riderId, CancellationToken ct = default)
    {
        return await _context.Deliveries
            .Where(d => d.RiderId == riderId)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<List<Delivery>> GetAllAsync(
        Guid? riderId, DeliveryStatus? status, CancellationToken ct = default)
    {
        var query = _context.Deliveries.AsQueryable();
        if (riderId.HasValue)
            query = query.Where(d => d.RiderId == riderId);
        if (status.HasValue)
            query = query.Where(d => d.Status == status);
        return await query
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task AddAsync(Delivery delivery, CancellationToken ct = default)
        => await _context.Deliveries.AddAsync(delivery, ct);

    public async Task SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);
}
