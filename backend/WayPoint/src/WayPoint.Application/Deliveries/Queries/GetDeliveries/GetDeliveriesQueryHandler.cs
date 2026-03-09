using MediatR;
using WayPoint.Application.Common.Interfaces.Repositories;
using WayPoint.Application.Common.Mappings;
using WayPoint.Application.Deliveries.DTOs;
using WayPoint.Domain.Common;

namespace WayPoint.Application.Deliveries.Queries.GetDeliveries;

public class GetDeliveriesQueryHandler
    : IRequestHandler<GetDeliveriesQuery, Result<List<DeliveryDto>>>
{
    private readonly IDeliveryRepository _repo;
    private readonly DeliveryMapper _mapper;

    public GetDeliveriesQueryHandler(IDeliveryRepository repo, DeliveryMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<Result<List<DeliveryDto>>> Handle(
        GetDeliveriesQuery request, CancellationToken ct)
    {
        var deliveries = await _repo.GetAllAsync(request.RiderId, request.Status, ct);

        var deliveryDtos = deliveries.Select(delivery => _mapper.ToDto(delivery)).ToList();

        return Result<List<DeliveryDto>>.Success(deliveryDtos);
    }
}
