using MediatR;

using WayPoint.Application.Riders.DTOs;
using WayPoint.Domain.Common;
using WayPoint.Domain.Enums;

namespace WayPoint.Application.Riders.Queries.GetRiders;

public record GetRidersQuery(RiderStatus? Status = null) : IRequest<Result<List<RiderDto>>>;
