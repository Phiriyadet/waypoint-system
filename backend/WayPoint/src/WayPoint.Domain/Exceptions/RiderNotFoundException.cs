namespace WayPoint.Domain.Exceptions;

public class RiderNotFoundException(Guid id)
    : Exception($"Rider with ID '{id}' was not found.");
