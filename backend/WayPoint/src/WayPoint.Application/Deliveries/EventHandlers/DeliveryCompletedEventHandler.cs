using MediatR;
using Microsoft.Extensions.Logging;
using WayPoint.Application.Common.Interfaces.Repositories;
using WayPoint.Application.Locations.Commands.UpdateVerifiedLocation;
using WayPoint.Domain.Events;

namespace WayPoint.Application.Deliveries.EventHandlers;

public class DeliveryCompletedEventHandler
    : INotificationHandler<DeliveryCompletedEvent>
{
    private readonly ILocationRepository _locationRepo;
    private readonly ISender _mediator;
    private readonly ILogger<DeliveryCompletedEventHandler> _logger;

    public DeliveryCompletedEventHandler(
        ILocationRepository locationRepo,
        ISender mediator,
        ILogger<DeliveryCompletedEventHandler> logger)
    {
        _locationRepo = locationRepo;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Handle(DeliveryCompletedEvent notification, CancellationToken ct)
    {
        var location = await _locationRepo.GetByIdAsync(notification.LocationId, ct);
        if (location is null)
        {
            _logger.LogWarning(
                "Location {LocationId} not found for DeliveryCompletedEvent",
                notification.LocationId);
            return;
        }

        // ✅ Let Domain decide what to do with GPS data
        var result = location.LearnFromDelivery(
            notification.ActualDeliveryCoordinate,
            notification.GpsAccuracy);

        if (result.ShouldUpdateVerifiedCoordinate)
        {
            // Domain decided: significant offset → update verified coordinate
            await _mediator.Send(new UpdateVerifiedLocationCommand(
                notification.LocationId,
                result.NewCoordinate.Y,
                result.NewCoordinate.X,
                result.OffsetMeters), ct);

            _logger.LogInformation(
                "Location {LocationId} VerifiedCoordinate updated. Offset={Offset:F0}m",
                notification.LocationId, result.OffsetMeters);
        }
        else
        {
            // Domain decided: small offset → just increment score (already done in Domain)
            await _locationRepo.SaveChangesAsync(ct);

            _logger.LogInformation(
                "Location {LocationId} ConfidenceScore incremented. Offset={Offset:F0}m",
                notification.LocationId, result.OffsetMeters);
        }
    }
}