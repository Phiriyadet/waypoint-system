using MediatR;
using WayPoint.Application.Common.Interfaces.Infrastructure;
using WayPoint.Application.Riders.DTOs;
using WayPoint.Domain.Common;

namespace WayPoint.Application.Riders.Queries.GetRiderById;

public record GetRiderByIdQuery(Guid Id)
    : IRequest<Result<RiderDto>>, ICacheable
{
    public string CacheKey => $"rider:{Id}";
}
