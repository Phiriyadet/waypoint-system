using MediatR;
using WayPoint.Application.Locations.DTOs;
using WayPoint.Domain.Common;

namespace WayPoint.Application.Locations.Queries.SearchLocations;

public record SearchLocationsQuery(string Query, int Limit = 10) : IRequest<Result<List<LocationDto>>>;
