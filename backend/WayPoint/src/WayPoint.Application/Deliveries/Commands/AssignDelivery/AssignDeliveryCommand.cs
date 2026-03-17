using MediatR;

using WayPoint.Application.Deliveries.DTOs;
using WayPoint.Domain.Common;

namespace WayPoint.Application.Deliveries.Commands.AssignRider;

public record AssignDeliveryCommand(
    Guid DeliveryId,
    Guid RiderId
) : IRequest<Result<DeliveryDto>>;

