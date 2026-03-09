using MediatR;
using WayPoint.Application.Common.Interfaces.Repositories;
using WayPoint.Application.Common.Mappings;
using WayPoint.Application.Riders.DTOs;
using WayPoint.Domain.Common;
using WayPoint.Domain.Enums;

namespace WayPoint.Application.Riders.Queries.GetAvailableRiders;

public class GetAvailableRidersQueryHandler
    : IRequestHandler<GetAvailableRidersQuery, Result<List<RiderDto>>>
{
    private readonly IRiderRepository _repo;
    private readonly RiderMapper _mapper;

    public GetAvailableRidersQueryHandler(IRiderRepository repo, RiderMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<Result<List<RiderDto>>> Handle(
        GetAvailableRidersQuery request, CancellationToken ct)
    {
        var riders = await _repo.GetByStatusAsync(RiderStatus.Available, ct);

        var riderDtos = riders.Select(rider => _mapper.ToDto(rider)).ToList();

        return Result<List<RiderDto>>.Success(riderDtos);
    }
}
