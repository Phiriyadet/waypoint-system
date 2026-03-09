using MediatR;
using WayPoint.Application.Deliveries.DTOs;
using WayPoint.Domain.Common;

namespace WayPoint.Application.Deliveries.Commands.StartDelivery;

public record StartDeliveryCommand(
    Guid DeliveryId,
    Guid RiderId
) : IRequest<Result<DeliveryDto>>;
