using MediatR;
using WayPoint.Application.Common.Interfaces.Repositories;
using WayPoint.Application.Common.Mappings;
using WayPoint.Application.Locations.DTOs;
using WayPoint.Domain.Common;
using WayPoint.Domain.Exceptions;

namespace WayPoint.Application.Locations.Queries.GetLocationById;

public class GetLocationByIdQueryHandler
    : IRequestHandler<GetLocationByIdQuery, Result<LocationDto>>
{
    private readonly ILocationRepository _repo;
    private readonly LocationMapper _mapper;

    public GetLocationByIdQueryHandler(ILocationRepository repo, LocationMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<Result<LocationDto>> Handle(
        GetLocationByIdQuery request, CancellationToken ct)
    {
        var location = await _repo.GetByIdAsync(request.Id, ct)
            ?? throw new LocationNotFoundException(request.Id);

        return Result<LocationDto>.Success(_mapper.ToDto(location));
    }
}
