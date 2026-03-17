using MediatR;

using WayPoint.Application.Common.Interfaces.Repositories;
using WayPoint.Application.Common.Mappings;
using WayPoint.Application.Deliveries.DTOs;
using WayPoint.Domain.Common;
using WayPoint.Domain.Exceptions;

namespace WayPoint.Application.Deliveries.Commands.AssignRider;

public class AssignDeliveryCommandHandler : IRequestHandler<AssignDeliveryCommand, Result<DeliveryDto>>

{
    private readonly IDeliveryRepository _deliveryRepo;
    private readonly IRiderRepository _riderRepo;
    private readonly DeliveryMapper _mapper;

    public AssignDeliveryCommandHandler(
        IDeliveryRepository deliveryRepo,
        IRiderRepository riderRepo,
        DeliveryMapper mapper)
    {
        _deliveryRepo = deliveryRepo;
        _riderRepo = riderRepo;
        _mapper = mapper;
    }

    public async Task<Result<DeliveryDto>> Handle(
    AssignDeliveryCommand request, CancellationToken ct)
    {
        var delivery = await _deliveryRepo.GetByIdAsync(request.DeliveryId, ct)
        ?? throw new DeliveryNotFoundException(request.DeliveryId);

        var rider = await _riderRepo.GetByIdAsync(request.RiderId, ct)
        ?? throw new RiderNotFoundException(request.RiderId);

        // ✅ Let Domain handle all business rules
        var result = delivery.AssignRider(request.RiderId, rider);

        if (result.IsFailure)

            return Result<DeliveryDto>.Failure(result.Error);

        await _deliveryRepo.SaveChangesAsync(ct);

        return Result<DeliveryDto>.Success(_mapper.ToDto(delivery));
    }
}
