using MediatR;
using WayPoint.Application.Deliveries.DTOs;
using WayPoint.Domain.Common;

namespace WayPoint.Application.Deliveries.Commands.CreateDelivery;

public record CreateDeliveryCommand(
    Guid LocationId,
    string RecipientName,
    string RecipientPhone
) : IRequest<Result<DeliveryDto>>;
