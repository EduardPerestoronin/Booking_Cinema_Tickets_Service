using Domain.Cinema_Booking.Base;
using Domain.Cinema_Booking.Enums;
using Domain.Cinema_Booking.Exceptions;

namespace Domain.Cinema_Booking;
public class Booking : Entity<Guid>
{

    private readonly ICollection<Seat> _seats = [];

    public User User { get; } = default!;

    public Session Session { get; } = default!;
    public IReadOnlyCollection<Seat> Seats => _seats.ToList().AsReadOnly();

    public DateTime CreatedAt { get; }
    public DateTime ExpiresAt { get; }

    public BookingStatus Status { get; private set; }
    protected Booking() { }

    public Booking(User user, Session session, IEnumerable<Seat> seats, DateTime createdAt, DateTime expiresAt)
        : this(Guid.NewGuid(), user, session, seats, createdAt, expiresAt, BookingStatus.Active) { }

    protected Booking(
        Guid id,
        User user,
        Session session,
        IEnumerable<Seat> seats,
        DateTime createdAt,
        DateTime expiresAt,
        BookingStatus status)
        : base(id)
    {
        User = user ?? throw new ArgumentNullValueException(nameof(user));
        Session = session ?? throw new ArgumentNullValueException(nameof(session));
        if (seats == null) throw new ArgumentNullValueException(nameof(seats));

        var seatsList = seats.ToList();
        if (seatsList.Count == 0)
            throw new EmptySeatsSelectionException();

        foreach (var seat in seatsList)
        {
            if (seat == null) throw new ArgumentNullValueException(nameof(seat));
            if (seat.Hall.Id != session.Hall.Id)
                throw new SeatNotInHallException(seat, session.Hall);
            _seats.Add(seat);
        }

        CreatedAt = createdAt;
        ExpiresAt = expiresAt;
        Status = status;
    }

    public bool IsActive()
    {
        if (Status != BookingStatus.Active) return false;
        if (DateTime.UtcNow >= ExpiresAt)
        {
            Status = BookingStatus.Expired;
            return false;
        }
        return true;
    }

    public void Confirm()
    {
        if (DateTime.UtcNow >= ExpiresAt)
        {
            Status = BookingStatus.Expired;
            throw new BookingExpiredException(this);
        }

        if (Status != BookingStatus.Active)
            throw new BookingNotActiveException(this);

        Status = BookingStatus.Confirmed;
    }

    public void Cancel()
    {
        if (Status == BookingStatus.Cancelled) return;
        if (Status == BookingStatus.Confirmed)
            throw new InvalidOperationException("Cannot cancel a confirmed booking.");
        if (Status == BookingStatus.Expired)
            throw new BookingExpiredException(this);

        Status = BookingStatus.Cancelled;
    }
}
