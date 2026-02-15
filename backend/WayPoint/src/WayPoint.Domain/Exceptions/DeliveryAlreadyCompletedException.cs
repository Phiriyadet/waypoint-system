namespace WayPoint.Domain.Exceptions;

public class DeliveryAlreadyCompletedException(Guid id) : Exception($"Delivery with ID '{id}' has already been completed.");
