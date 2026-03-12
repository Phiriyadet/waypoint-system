using WayPoint.Application.Riders.DTOs;

namespace WayPoint.Application.Common.Interfaces.ExternalServices;

public interface IRiderLocationClient
{
    Task LocationUpdated(RiderLocationDto location);
}
