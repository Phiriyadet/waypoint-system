using MediatR;

using WayPoint.Application.Common.Interfaces.Repositories;
using WayPoint.Application.Common.Mappings;
using WayPoint.Application.Riders.DTOs;
using WayPoint.Domain.Common;

namespace WayPoint.Application.Riders.Queries.GetRiders;

public class GetRidersQueryHandler
    : IRequestHandler<GetRidersQuery, Result<List<RiderDto>>>
{
    private readonly IRiderRepository _repo;
    private readonly RiderMapper _mapper;

    public GetRidersQueryHandler(IRiderRepository repo, RiderMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<Result<List<RiderDto>>> Handle(
        GetRidersQuery request, CancellationToken ct)
    {
        if (request.Status is null)
        {
            return Result<List<RiderDto>>.Success(new List<RiderDto>());
        }

        var riders = await _repo.GetByStatusAsync(request.Status.Value, ct);

        var riderDtos = riders.Select(rider => _mapper.ToDto(rider)).ToList();

        return Result<List<RiderDto>>.Success(riderDtos);
    }
}
