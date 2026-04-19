using Domain.Cinema_Booking.Exceptions;
using Domain.ValueObject;
using static System.Collections.Specialized.BitVector32;

namespace Domain.Cinema_Booking;

public class User
{
    public Guid Id { get; private set; }
    public Username Username { get; private set; }
    public Email Email { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private readonly List<Booking> _bookings = new();
    public IReadOnlyCollection<Booking> Bookings => _bookings;

    protected User() { }

    public User(Guid id, Username username, Email email, DateTime createdAt)
    {
        Id = id;
        Username = username ?? throw new ArgumentNullValueException(nameof(username));
        Email = email ?? throw new ArgumentNullValueException(nameof(email));
        CreatedAt = createdAt;
    }

    public Booking CreateBooking(Session session, Seat seat, DateTime expiresAt)
    {
        var booking = new Booking(Guid.NewGuid(), this, session, seat, expiresAt);
        _bookings.Add(booking);
        return booking;
    }
}