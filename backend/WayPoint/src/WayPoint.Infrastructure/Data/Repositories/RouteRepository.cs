using Microsoft.EntityFrameworkCore;

using WayPoint.Application.Common.Interfaces.Repositories;
using WayPoint.Domain.Entities;

namespace WayPoint.Infrastructure.Data.Repositories;

public class RouteRepository : IRouteRepository
{
    private readonly ApplicationDbContext _context;

    public RouteRepository(ApplicationDbContext context) => _context = context;

    public async Task<Route?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Routes
            .Include(r => r.Waypoints)
            .FirstOrDefaultAsync(r => r.Id == id, ct);

    public async Task AddAsync(Route route, CancellationToken ct = default)
        => await _context.Routes.AddAsync(route, ct);

    public async Task SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);
}
