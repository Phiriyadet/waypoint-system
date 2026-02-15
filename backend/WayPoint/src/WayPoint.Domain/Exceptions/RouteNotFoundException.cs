namespace WayPoint.Domain.Exceptions;

public class RouteNotFoundException(Guid id)
    : Exception($"Route with ID '{id}' was not found.");
