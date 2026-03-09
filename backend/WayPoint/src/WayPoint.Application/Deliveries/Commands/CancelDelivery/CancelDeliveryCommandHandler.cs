using MediatR;
using WayPoint.Application.Common.Interfaces.Repositories;
using WayPoint.Application.Common.Mappings;
using WayPoint.Application.Deliveries.DTOs;
using WayPoint.Domain.Common;
using WayPoint.Domain.Exceptions;

namespace WayPoint.Application.Deliveries.Commands.CancelDelivery;

public class CancelDeliveryCommandHandler
    : IRequestHandler<CancelDeliveryCommand, Result<DeliveryDto>>
{
    private readonly IDeliveryRepository _deliveryRepo;
    private readonly DeliveryMapper _mapper;

    public CancelDeliveryCommandHandler(
        IDeliveryRepository deliveryRepo, DeliveryMapper mapper)
    {
        _deliveryRepo = deliveryRepo;
        _mapper = mapper;
    }

    public async Task<Result<DeliveryDto>> Handle(
        CancelDeliveryCommand request, CancellationToken ct)
    {
        var delivery = await _deliveryRepo.GetByIdAsync(request.DeliveryId, ct)
            ?? throw new DeliveryNotFoundException(request.DeliveryId);

        delivery.Cancel(request.Reason);
        await _deliveryRepo.SaveChangesAsync(ct);

        return Result<DeliveryDto>.Success(_mapper.ToDto(delivery));
    }
}
