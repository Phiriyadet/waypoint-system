using MediatR;
using WayPoint.Application.Common.Interfaces.ExternalServices;
using WayPoint.Application.Common.Interfaces.Repositories;
using WayPoint.Application.Common.Mappings;
using WayPoint.Application.Locations.DTOs;
using WayPoint.Domain.Common;
using WayPoint.Domain.Entities;
using WayPoint.Domain.ValueObjects;

namespace WayPoint.Application.Locations.Commands.CreateLocation;

public class CreateLocationCommandHandler : IRequestHandler<CreateLocationCommand, Result<LocationDto>>
{
    private readonly ILocationRepository _repo;
    private readonly IGeocodingService _geocoding;
    private readonly LocationMapper _mapper;

    public CreateLocationCommandHandler(
        ILocationRepository repo,
        IGeocodingService geocoding,
        LocationMapper mapper)
    {
        _repo = repo;
        _geocoding = geocoding;
        _mapper = mapper;
    }

    public async Task<Result<LocationDto>> Handle(
        CreateLocationCommand request, CancellationToken ct)
    {
        var address = Address.Create(
            request.AddressInfo, request.Subdistrict, request.District,
            request.Province, request.PostalCode, request.MoreInfo);

        // สางหา location ในรัศมี 50m ก่อนสร้างใหม่ (spatial dedup)
        var geocoded = await _geocoding.GeocodeAsync(address.ToGeocodingString(), ct);
        if (geocoded is null)
            return Result<LocationDto>.Failure("ไม่สามารถ geocode ที่อยู่นี้ได้");

        var existing = await _repo.FindNearbyAsync(geocoded.Coordinate, 50, ct);
        if (existing is not null)
            return Result<LocationDto>.Success(_mapper.ToDto(existing));

        var location = Location.Create(
            address, geocoded.Coordinate,
            request.PlaceName, request.AccessNotes);

        await _repo.AddAsync(location, ct);
        await _repo.SaveChangesAsync(ct);

        return Result<LocationDto>.Success(_mapper.ToDto(location));
    }
}