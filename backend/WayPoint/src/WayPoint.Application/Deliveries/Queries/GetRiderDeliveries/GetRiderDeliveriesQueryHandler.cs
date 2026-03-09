using MediatR;
using WayPoint.Application.Common.Interfaces.Repositories;
using WayPoint.Application.Common.Mappings;
using WayPoint.Application.Deliveries.DTOs;
using WayPoint.Domain.Common;

namespace WayPoint.Application.Deliveries.Queries.GetRiderDeliveries;

public class GetRiderDeliveriesQueryHandler
    : IRequestHandler<GetRiderDeliveriesQuery, Result<List<DeliveryDto>>>
{
    private readonly IDeliveryRepository _repo;
    private readonly DeliveryMapper _mapper;

    public GetRiderDeliveriesQueryHandler(IDeliveryRepository repo, DeliveryMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<Result<List<DeliveryDto>>> Handle(
        GetRiderDeliveriesQuery request, CancellationToken ct)
    {
        var deliveries = await _repo.GetByRiderIdAsync(request.RiderId, ct);

        var deliveryDtos = deliveries.Select(delivery => _mapper.ToDto(delivery)).ToList();

        return Result<List<DeliveryDto>>.Success(deliveryDtos);
    }
}
