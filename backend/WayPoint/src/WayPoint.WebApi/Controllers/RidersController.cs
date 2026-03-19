using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WayPoint.Application.Deliveries.DTOs;
using WayPoint.Application.Deliveries.Queries.GetRiderDeliveries;
using WayPoint.Application.Riders.Commands.CreateRider;
using WayPoint.Application.Riders.Commands.UpdateRiderLocation;
using WayPoint.Application.Riders.Commands.UpdateRiderStatus;
using WayPoint.Application.Riders.DTOs;
using WayPoint.Application.Riders.Queries.GetRiderById;
using WayPoint.Application.Riders.Queries.GetRiders;
using WayPoint.Domain.Enums;

namespace WayPoint.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RidersController : ControllerBase
{
    private readonly IMediator _mediator;

    public RidersController(IMediator mediator) => _mediator = mediator;

    /// <summary>Create rider account</summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<RiderDto>> Create(
        [FromBody] CreateRiderCommand command,
        CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value)
            : BadRequest(new { error = result.Error });
    }

    /// <summary>Get rider by ID</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RiderDto>> GetById(
        Guid id,
        CancellationToken ct)
    {
        var result = await _mediator.Send(new GetRiderByIdQuery(id), ct);
        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(new { error = result.Error });
    }

    /// <summary>List riders with status filter</summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<RiderDto>>> List(
        [FromQuery] RiderStatus? status,
        CancellationToken ct)
    {
        var query = new GetRidersQuery(status);
        var result = await _mediator.Send(query, ct);
        return Ok(result);
    }

    /// <summary>Get rider's assigned deliveries</summary>
    [HttpGet("{id:guid}/deliveries")]
    [Authorize(Roles = "Rider")]
    public async Task<ActionResult<List<DeliveryDto>>> GetDeliveries(
        Guid id,
        CancellationToken ct)
    {
        var query = new GetRiderDeliveriesQuery(id);
        var result = await _mediator.Send(query, ct);
        return Ok(result);
    }

    /// <summary>Update rider GPS location</summary>
    [HttpPatch("{id:guid}/location")]
    [Authorize(Roles = "Rider")]
    public async Task<ActionResult> UpdateLocation(
        Guid id,
        [FromBody] UpdateRiderLocationCommand command,
        CancellationToken ct)
    {
        if (id != command.RiderId)
            return BadRequest(new { error = "ID mismatch" });

        var result = await _mediator.Send(command, ct);
        return result.IsSuccess
            ? Ok(new { message = "Location updated" })
            : BadRequest(new { error = result.Error });
    }

    /// <summary>Change rider status</summary>
    [HttpPatch("{id:guid}/status")]
    [Authorize(Roles = "Rider")]
    public async Task<ActionResult> UpdateStatus(
        Guid id,
        [FromBody] UpdateRiderStatusCommand command,
        CancellationToken ct)
    {
        if (id != command.RiderId)
            return BadRequest(new { error = "ID mismatch" });

        var result = await _mediator.Send(command, ct);
        return result.IsSuccess
            ? Ok(new { message = "Status updated" })
            : BadRequest(new { error = result.Error });
    }
}
