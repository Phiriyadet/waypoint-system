using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WayPoint.Application.Routes.Commands.OptimizeRoute;
using WayPoint.Application.Routes.DTOs;
using WayPoint.Application.Routes.Queries.GetRouteById;

namespace WayPoint.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RoutesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoutesController(IMediator mediator) => _mediator = mediator;

    /// <summary>Optimize route for rider</summary>
    [HttpPost("optimize")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<RouteDto>> Optimize(
        [FromBody] OptimizeRouteCommand command,
        CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(new { error = result.Error });
    }

    /// <summary>Get route by ID</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RouteDto>> GetById(
        Guid id,
        CancellationToken ct)
    {
        var result = await _mediator.Send(new GetRouteByIdQuery(id), ct);
        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(new { error = result.Error });
    }
}
