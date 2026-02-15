namespace WayPoint.Domain.Exceptions;

public class InvalidCoordinateException(string message)
    : Exception(message);
