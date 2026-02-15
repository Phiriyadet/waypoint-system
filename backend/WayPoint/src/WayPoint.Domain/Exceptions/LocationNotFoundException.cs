namespace WayPoint.Domain.Exceptions;

public class LocationNotFoundException(Guid id)
    : Exception($"Location with ID '{id}' was not found.");
