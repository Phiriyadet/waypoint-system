using MediatR;
using WayPoint.Application.Common.Interfaces.Repositories;
using WayPoint.Application.Common.Mappings;
using WayPoint.Application.Deliveries.DTOs;
using WayPoint.Domain.Common;
using WayPoint.Domain.Exceptions;

namespace WayPoint.Application.Deliveries.Commands.StartDelivery;

public class StartDeliveryCommandHandler : IRequestHandler<StartDeliveryCommand, Result<DeliveryDto>>

{

    private readonly IDeliveryRepository _deliveryRepo;
    private readonly DeliveryMapper _mapper;

    public StartDeliveryCommandHandler(IDeliveryRepository deliveryRepo, DeliveryMapper mapper)
    {
        _deliveryRepo = deliveryRepo;
        _mapper = mapper;
    }

    public async Task<Result<DeliveryDto>> Handle(
    StartDeliveryCommand request, CancellationToken ct)

    {

        var delivery = await _deliveryRepo.GetByIdAsync(request.DeliveryId, ct)

        ?? throw new DeliveryNotFoundException(request.DeliveryId);

        // ✅ Domain handles authorization + state validation
        var result = delivery.StartDelivery(request.RiderId);

        if (result.IsFailure)
            return Result<DeliveryDto>.Failure(result.Error);

        await _deliveryRepo.SaveChangesAsync(ct);

        return Result<DeliveryDto>.Success(_mapper.ToDto(delivery));

    }
}