namespace WayPoint.Domain.Common;

/// <summary>
/// Base class for Value Objects in DDD
/// Value objects are immutable and have no identity
/// They are compared by their property values, not by ID
/// </summary>
public abstract class ValueObject
{
    /// <summary>
    /// Get the components that define equality for this value object
    /// </summary>
    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
            return false;

        var other = (ValueObject)obj;

        return GetEqualityComponents()
            .SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x?.GetHashCode() ?? 0)
            .Aggregate((x, y) => x ^ y);
    }

    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        if (left is null && right is null)
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    public static bool operator !=(ValueObject? left, ValueObject? right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Create a copy of this value object
    /// </summary>
    protected ValueObject GetCopy()
    {
        return (ValueObject)MemberwiseClone();
    }
}