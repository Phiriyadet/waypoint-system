using MediatR;
using WayPoint.Application.Locations.DTOs;
using WayPoint.Domain.Common;

namespace WayPoint.Application.Locations.Commands.CreateLocation;

public record CreateLocationCommand(
    string AddressInfo,
    string Subdistrict,
    string District,
    string Province,
    string PostalCode,
    string? MoreInfo,
    string? PlaceName,
    string? AccessNotes
) : IRequest<Result<LocationDto>>;
