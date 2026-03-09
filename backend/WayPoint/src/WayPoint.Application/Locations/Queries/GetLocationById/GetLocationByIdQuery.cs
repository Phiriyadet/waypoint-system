using MediatR;
using WayPoint.Application.Common.Interfaces.Infrastructure;
using WayPoint.Application.Locations.DTOs;
using WayPoint.Domain.Common;

namespace WayPoint.Application.Locations.Queries.GetLocationById;

public record GetLocationByIdQuery(Guid Id)
    : IRequest<Result<LocationDto>>, ICacheable
{
    public string CacheKey => $"location:{Id}";
}