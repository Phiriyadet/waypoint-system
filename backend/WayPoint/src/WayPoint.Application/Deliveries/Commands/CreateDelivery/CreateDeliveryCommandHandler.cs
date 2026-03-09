using MediatR;
using WayPoint.Application.Common.Interfaces.Repositories;
using WayPoint.Application.Common.Mappings;
using WayPoint.Application.Deliveries.DTOs;
using WayPoint.Domain.Common;
using WayPoint.Domain.Entities;
using WayPoint.Domain.Exceptions;

namespace WayPoint.Application.Deliveries.Commands.CreateDelivery;

public class CreateDeliveryCommandHandler : IRequestHandler<CreateDeliveryCommand, Result<DeliveryDto>>
{
    private readonly IDeliveryRepository _deliveryRepo;
    private readonly ILocationRepository _locationRepo;
    private readonly DeliveryMapper _mapper;

    public CreateDeliveryCommandHandler(
        IDeliveryRepository deliveryRepo,
        ILocationRepository locationRepo,
        DeliveryMapper mapper)
    {
        _deliveryRepo = deliveryRepo;
        _locationRepo = locationRepo;
        _mapper = mapper;
    }

    public async Task<Result<DeliveryDto>> Handle(
        CreateDeliveryCommand request, CancellationToken ct)
    {
        var location = await _locationRepo.GetByIdAsync(request.LocationId, ct)
            ?? throw new LocationNotFoundException(request.LocationId);

        var delivery = Delivery.Create(
            request.LocationId,
            request.RecipientName,
            request.RecipientPhone);

        await _deliveryRepo.AddAsync(delivery, ct);
        await _deliveryRepo.SaveChangesAsync(ct);

        return Result<DeliveryDto>.Success(_mapper.ToDto(delivery));
    }
}
