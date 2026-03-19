using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WayPoint.Application.Locations.Commands.CreateLocation;
using WayPoint.Application.Locations.DTOs;
using WayPoint.Application.Locations.Queries.GetLocationById;
using WayPoint.Application.Locations.Queries.GetNearbyLocations;
using WayPoint.Application.Locations.Queries.SearchLocations;

namespace WayPoint.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LocationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public LocationsController(IMediator mediator) => _mediator = mediator;

    /// <summary>Create new location with auto-geocoding</summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<LocationDto>> Create(
        [FromBody] CreateLocationCommand command,
        CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value)
            : BadRequest(new { error = result.Error });
    }

    /// <summary>Get location by ID</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<LocationDto>> GetById(
        Guid id,
        CancellationToken ct)
    {
        var result = await _mediator.Send(new GetLocationByIdQuery(id), ct);
        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(new { error = result.Error });
    }

    /// <summary>Find locations within radius</summary>
    [HttpGet("nearby")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<LocationDto>>> GetNearby(
        [FromQuery] double lat,
        [FromQuery] double lng,
        [FromQuery] double radius = 1000,
        CancellationToken ct = default)
    {
        var query = new GetNearbyLocationsQuery(lat, lng, radius);
        var result = await _mediator.Send(query, ct);
        return Ok(result);
    }

    /// <summary>Search locations by name</summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<LocationDto>>> Search(
        [FromQuery] string? search,
        CancellationToken ct)
    {
        var query = new SearchLocationsQuery(search);
        var result = await _mediator.Send(query, ct);
        return Ok(result);
    }
}
