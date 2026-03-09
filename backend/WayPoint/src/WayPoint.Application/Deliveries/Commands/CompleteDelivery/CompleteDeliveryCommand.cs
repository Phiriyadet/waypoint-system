using MediatR;
using WayPoint.Application.Deliveries.DTOs;
using WayPoint.Domain.Common;

namespace WayPoint.Application.Deliveries.Commands.CompleteDelivery;

public record CompleteDeliveryCommand(
    Guid DeliveryId,
    Guid RiderId,
    double Latitude,
    double Longitude,
    double AccuracyMeters
) : IRequest<Result<DeliveryDto>>;