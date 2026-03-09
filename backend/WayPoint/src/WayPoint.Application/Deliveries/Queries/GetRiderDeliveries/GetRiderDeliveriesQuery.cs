using MediatR;
using WayPoint.Application.Deliveries.DTOs;
using WayPoint.Domain.Common;

namespace WayPoint.Application.Deliveries.Queries.GetRiderDeliveries;

public record GetRiderDeliveriesQuery(Guid RiderId)
    : IRequest<Result<List<DeliveryDto>>>;
