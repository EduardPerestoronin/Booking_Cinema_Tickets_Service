using Domain.ValueObject;
using Domain.Cinema_Booking.Exceptions;

namespace Domain.Cinema_Booking;

public class Session
{
    public Guid Id { get; private set; }
    public Movie Movie { get; private set; }
    public Hall Hall { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }

    private readonly List<Booking> _bookings = new();
    public IReadOnlyCollection<Booking> Bookings => _bookings;

    protected Session() { }

    public Session(Guid id, Movie movie, Hall hall, DateTime startTime, DateTime endTime)
    {
        Id = id;
        Movie = movie ?? throw new ArgumentNullException(nameof(movie));
        Hall = hall ?? throw new ArgumentNullException(nameof(hall));
        StartTime = startTime;
        EndTime = endTime > startTime ? endTime : throw new ArgumentException("EndTime must be after StartTime");
    }

    public bool IsSeatAvailable(Seat seat)
    {
        return !_bookings.Any(b => b.Seat.Id == seat.Id && b.IsActive());
    }

    public Booking BookSeat(User user, Seat seat, DateTime expiresAt)
    {
        if (!IsSeatAvailable(seat))
            throw new SeatAlreadyBookedException(Id, seat.SeatNumber.Value);

        var booking = new Booking(Guid.NewGuid(), user, this, seat, expiresAt);
        _bookings.Add(booking);
        return booking;
    }
}