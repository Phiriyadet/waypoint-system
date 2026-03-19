using System.Security.Claims;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WayPoint.Application.Deliveries.Commands.AssignRider;
using WayPoint.Application.Deliveries.Commands.CancelDelivery;
using WayPoint.Application.Deliveries.Commands.CompleteDelivery;
using WayPoint.Application.Deliveries.Commands.CreateDelivery;
using WayPoint.Application.Deliveries.Commands.StartDelivery;
using WayPoint.Application.Deliveries.DTOs;
using WayPoint.Application.Deliveries.Queries.GetDeliveries;
using WayPoint.Application.Deliveries.Queries.GetDeliveryById;
using WayPoint.Domain.Enums;

namespace WayPoint.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DeliveriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public DeliveriesController(IMediator mediator) => _mediator = mediator;

    /// <summary>Create new delivery</summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<DeliveryDto>> Create(
        [FromBody] CreateDeliveryCommand command,
        CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value)
            : BadRequest(new { error = result.Error });
    }

    /// <summary>Get delivery by ID</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DeliveryDto>> GetById(
        Guid id,
        CancellationToken ct)
    {
        var result = await _mediator.Send(new GetDeliveryByIdQuery(id), ct);
        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(new { error = result.Error });
    }

    /// <summary>List all deliveries with filters</summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<DeliveryDto>>> List(
        [FromQuery] Guid? riderId,
        [FromQuery] DeliveryStatus? status,
        CancellationToken ct)
    {
        var query = new GetDeliveriesQuery(riderId, status);
        var result = await _mediator.Send(query, ct);
        return Ok(result);
    }

    /// <summary>Assign delivery to rider</summary>
    [HttpPatch("{id:guid}/assign")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Assign(
        Guid id,
        [FromBody] AssignDeliveryCommand command,
        CancellationToken ct)
    {
        if (id != command.DeliveryId)
            return BadRequest(new { error = "ID mismatch" });

        var result = await _mediator.Send(command, ct);
        return result.IsSuccess
            ? Ok(new { message = "Delivery assigned" })
            : BadRequest(new { error = result.Error });
    }

    /// <summary>Rider starts delivery</summary>
    [HttpPatch("{id:guid}/start")]
    [Authorize(Roles = "Rider")]
    public async Task<ActionResult> Start(
        Guid id,
        CancellationToken ct)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userIdClaim == null) return Unauthorized();

        var riderId = Guid.Parse(userIdClaim);

        var result = await _mediator.Send(new StartDeliveryCommand(id, riderId), ct);
        return result.IsSuccess
            ? Ok(new { message = "Delivery started" })
            : BadRequest(new { error = result.Error });
    }

    /// <summary>Complete delivery with GPS</summary>
    [HttpPatch("{id:guid}/complete")]
    [Authorize(Roles = "Rider")]
    public async Task<ActionResult> Complete(
        Guid id,
        [FromBody] CompleteDeliveryCommand command,
        CancellationToken ct)
    {
        if (id != command.DeliveryId)
            return BadRequest(new { error = "ID mismatch" });

        var result = await _mediator.Send(command, ct);
        return result.IsSuccess
            ? Ok(new { message = "Delivery completed" })
            : BadRequest(new { error = result.Error });
    }

    /// <summary>Cancel delivery with reason</summary>
    [HttpPatch("{id:guid}/cancel")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Cancel(
        Guid id,
        [FromBody] CancelDeliveryCommand command,
        CancellationToken ct)
    {
        if (id != command.DeliveryId)
            return BadRequest(new { error = "ID mismatch" });

        var result = await _mediator.Send(command, ct);
        return result.IsSuccess
            ? Ok(new { message = "Delivery cancelled" })
            : BadRequest(new { error = result.Error });
    }
}
