using Domain.Cinema_Booking.Exceptions;

namespace Domain.Cinema_Booking;

public class Booking
{
    public Guid Id { get; private set; }
    public User User { get; private set; }
    public Session Session { get; private set; }
    public Seat Seat { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public bool IsConfirmed { get; private set; }

    protected Booking() { }

    public Booking(Guid id, User user, Session session, Seat seat, DateTime expiresAt)
    {
        Id = id;
        User = user ?? throw new ArgumentNullException(nameof(user));
        Session = session ?? throw new ArgumentNullException(nameof(session));
        Seat = seat ?? throw new ArgumentNullException(nameof(seat));
        CreatedAt = DateTime.UtcNow;
        ExpiresAt = expiresAt;
        IsConfirmed = false;
    }

    public bool IsActive()
    {
        return !IsConfirmed && DateTime.UtcNow < ExpiresAt;
    }

    public void Confirm()
    {
        if (DateTime.UtcNow > ExpiresAt)
            throw new BookingExpiredException(Id, ExpiresAt);

        IsConfirmed = true;
    }

    public void Cancel()
    {
        if (IsConfirmed)
            throw new InvalidOperationException("Cannot cancel confirmed booking");
    }
}