using MediatR;
using WayPoint.Application.Common.Interfaces.Repositories;
using WayPoint.Application.Common.Mappings;
using WayPoint.Application.Deliveries.DTOs;
using WayPoint.Domain.Common;
using WayPoint.Domain.Exceptions;

namespace WayPoint.Application.Deliveries.Queries.GetDeliveryById;

public class GetDeliveryByIdQueryHandler
    : IRequestHandler<GetDeliveryByIdQuery, Result<DeliveryDto>>
{
    private readonly IDeliveryRepository _repo;
    private readonly DeliveryMapper _mapper;

    public GetDeliveryByIdQueryHandler(IDeliveryRepository repo, DeliveryMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<Result<DeliveryDto>> Handle(
        GetDeliveryByIdQuery request, CancellationToken ct)
    {
        var delivery = await _repo.GetByIdAsync(request.Id, ct)
            ?? throw new DeliveryNotFoundException(request.Id);

        return Result<DeliveryDto>.Success(_mapper.ToDto(delivery));
    }
}
