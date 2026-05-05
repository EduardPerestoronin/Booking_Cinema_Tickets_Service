namespace Domain.Cinema_Booking.Base;

public abstract class Entity<TId>(TId id) where TId : struct, IEquatable<TId>
{
   
    public TId Id { get; } = id;

    protected Entity() : this(default!)
    {
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity<TId> other) return false;
        if (ReferenceEquals(this, other)) return true;
        if (GetType() != other.GetType()) return false;
        return Id.Equals(other.Id);
    }

    public override int GetHashCode()
        => HashCode.Combine(GetType(), Id);
}
