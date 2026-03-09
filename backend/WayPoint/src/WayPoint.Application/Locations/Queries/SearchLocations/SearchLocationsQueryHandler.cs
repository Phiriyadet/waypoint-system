using MediatR;
using WayPoint.Application.Common.Interfaces.Repositories;
using WayPoint.Application.Common.Mappings;
using WayPoint.Application.Locations.DTOs;
using WayPoint.Domain.Common;

namespace WayPoint.Application.Locations.Queries.SearchLocations;

public class SearchLocationsQueryHandler
    : IRequestHandler<SearchLocationsQuery, Result<List<LocationDto>>>
{
    private readonly ILocationRepository _repo;
    private readonly LocationMapper _mapper;

    public SearchLocationsQueryHandler(ILocationRepository repo, LocationMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<Result<List<LocationDto>>> Handle(
        SearchLocationsQuery request, CancellationToken ct)
    {
        var locations = await _repo.SearchAsync(request.Query, request.Limit, ct);

        var locationDtos = locations.Select(location => _mapper.ToDto(location)).ToList();

        return Result<List<LocationDto>>.Success(locationDtos);
    }
}
