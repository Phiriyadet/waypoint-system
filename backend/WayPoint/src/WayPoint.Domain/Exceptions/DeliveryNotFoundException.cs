namespace WayPoint.Domain.Exceptions;

public class DeliveryNotFoundException(Guid id)
    : Exception($"Delivery with ID '{id}' was not found.");
