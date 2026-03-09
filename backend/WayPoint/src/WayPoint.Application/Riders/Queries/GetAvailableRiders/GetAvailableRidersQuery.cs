using MediatR;
using WayPoint.Application.Riders.DTOs;
using WayPoint.Domain.Common;

namespace WayPoint.Application.Riders.Queries.GetAvailableRiders;

public record GetAvailableRidersQuery : IRequest<Result<List<RiderDto>>>;
