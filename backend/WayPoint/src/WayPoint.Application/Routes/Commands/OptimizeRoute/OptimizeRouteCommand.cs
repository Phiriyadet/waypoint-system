using MediatR;
using WayPoint.Application.Routes.DTOs;
using WayPoint.Domain.Common;

namespace WayPoint.Application.Routes.Commands.OptimizeRoute;

public record OptimizeRouteCommand(
    Guid RiderId,
    List<Guid> DeliveryIds,
    string Strategy
) : IRequest<Result<RouteDto>>;
