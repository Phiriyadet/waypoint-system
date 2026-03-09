using MediatR;
using WayPoint.Application.Common.Interfaces.Infrastructure;
using WayPoint.Application.Routes.DTOs;
using WayPoint.Domain.Common;

namespace WayPoint.Application.Routes.Queries.GetRouteById;

public record GetRouteByIdQuery(Guid Id)
    : IRequest<Result<RouteDto>>, ICacheable
{
    public string CacheKey => $"route:{Id}";
}
