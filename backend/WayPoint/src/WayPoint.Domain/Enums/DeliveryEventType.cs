namespace WayPoint.Domain.Enums;

public enum DeliveryEventType
{
    Created = 0,
    Assigned = 1,
    InTransit = 2,
    Arrived = 3,
    Completed = 4,
    Failed = 5,
    Cancelled = 6,
    Rescheduled = 7
}
