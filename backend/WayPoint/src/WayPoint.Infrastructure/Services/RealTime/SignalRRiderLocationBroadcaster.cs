using Microsoft.AspNetCore.SignalR;

using WayPoint.Application.Common.Interfaces.ExternalServices;
using WayPoint.Application.Riders.DTOs;

namespace WayPoint.Infrastructure.Services.RealTime;

public class SignalRRiderLocationBroadcaster : IRiderLocationBroadcaster
{
    private readonly IHubContext<Hub<IRiderLocationClient>, IRiderLocationClient> _hubContext;

    public SignalRRiderLocationBroadcaster(
        IHubContext<Hub<IRiderLocationClient>, IRiderLocationClient> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task BroadcastAsync(RiderLocationDto dto, CancellationToken ct)
    {
        // Type-safe call
        await _hubContext.Clients
            .Group(dto.RiderId.ToString())
            .LocationUpdated(dto);
    }
}
