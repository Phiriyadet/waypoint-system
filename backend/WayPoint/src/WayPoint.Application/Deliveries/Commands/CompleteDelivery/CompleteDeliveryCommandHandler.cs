using MediatR;
using NetTopologySuite.Geometries;
using WayPoint.Application.Common.Interfaces.Repositories;
using WayPoint.Application.Common.Mappings;
using WayPoint.Application.Deliveries.DTOs;
using WayPoint.Domain.Common;
using WayPoint.Domain.Exceptions;
using WayPoint.Domain.ValueObjects;

namespace WayPoint.Application.Deliveries.Commands.CompleteDelivery;

public class CompleteDeliveryCommandHandler : IRequestHandler<CompleteDeliveryCommand, Result<DeliveryDto>>

{

    private readonly IDeliveryRepository _deliveryRepo;
    private readonly DeliveryMapper _mapper;

    public CompleteDeliveryCommandHandler(

    IDeliveryRepository deliveryRepo, DeliveryMapper mapper)
    {

        _deliveryRepo = deliveryRepo;
        _mapper = mapper;

    }

    public async Task<Result<DeliveryDto>> Handle(CompleteDeliveryCommand request, CancellationToken ct)
    {

        var delivery = await _deliveryRepo.GetByIdAsync(request.DeliveryId, ct)

        ?? throw new DeliveryNotFoundException(request.DeliveryId);

        var coordinate = new Point(request.Longitude, request.Latitude) { SRID = 4326 };

        var accuracy = GpsAccuracy.Create(request.AccuracyMeters);

        // ✅ Domain handles authorization + completion logic
        var result = delivery.Complete(request.RiderId, coordinate, accuracy);

        if (result.IsFailure)
            return Result<DeliveryDto>.Failure(result.Error);

        await _deliveryRepo.SaveChangesAsync(ct); // → dispatch DeliveryCompletedEvent

        return Result<DeliveryDto>.Success(_mapper.ToDto(delivery));

    }
}