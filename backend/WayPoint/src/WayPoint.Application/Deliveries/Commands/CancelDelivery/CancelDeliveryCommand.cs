using MediatR;
using WayPoint.Application.Deliveries.DTOs;
using WayPoint.Domain.Common;

namespace WayPoint.Application.Deliveries.Commands.CancelDelivery;

public record CancelDeliveryCommand(
    Guid DeliveryId,
    string Reason
) : IRequest<Result<DeliveryDto>>;
