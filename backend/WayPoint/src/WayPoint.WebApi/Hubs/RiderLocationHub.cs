using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WayPoint.WebApi.Hubs;

/// <summary>
/// SignalR Hub for real-time rider location updates
/// Admin dashboard subscribes to track riders
/// </summary>
[Authorize]
public class RiderLocationHub : Hub
{
    /// <summary>Subscribe to a specific rider's location</summary>
    public async Task SubscribeToRider(string riderId)
        => await Groups.AddToGroupAsync(Context.ConnectionId, riderId);

    /// <summary>Unsubscribe from a rider</summary>
    public async Task UnsubscribeFromRider(string riderId)
        => await Groups.RemoveFromGroupAsync(Context.ConnectionId, riderId);

    /// <summary>Subscribe to all riders</summary>
    public async Task SubscribeToAllRiders()
        => await Groups.AddToGroupAsync(Context.ConnectionId, "all-riders");

    /// <summary>Unsubscribe from all riders</summary>
    public async Task UnsubscribeFromAllRiders()
        => await Groups.RemoveFromGroupAsync(Context.ConnectionId, "all-riders");

    public override async Task OnConnectedAsync()
    {
        // Log connection
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // Cleanup
        await base.OnDisconnectedAsync(exception);
    }
}
