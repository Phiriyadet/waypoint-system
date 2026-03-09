using MediatR;
using WayPoint.Application.Deliveries.DTOs;
using WayPoint.Domain.Common;
using WayPoint.Domain.Enums;

namespace WayPoint.Application.Deliveries.Queries.GetDeliveries;

public record GetDeliveriesQuery(
    Guid? RiderId,
    DeliveryStatus? Status
) : IRequest<Result<List<DeliveryDto>>>;