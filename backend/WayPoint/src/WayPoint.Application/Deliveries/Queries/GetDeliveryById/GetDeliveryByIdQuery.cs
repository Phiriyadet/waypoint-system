using MediatR;
using WayPoint.Application.Deliveries.DTOs;
using WayPoint.Domain.Common;

namespace WayPoint.Application.Deliveries.Queries.GetDeliveryById;

public record GetDeliveryByIdQuery(Guid Id) : IRequest<Result<DeliveryDto>>;
