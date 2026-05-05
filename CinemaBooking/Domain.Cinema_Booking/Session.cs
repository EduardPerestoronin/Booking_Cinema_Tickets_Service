using Domain.Cinema_Booking.Base;
using Domain.Cinema_Booking.Exceptions;

namespace Domain.Cinema_Booking;

public class Session : Entity<Guid>
{

    private readonly ICollection<Booking> _bookings = [];

    public Movie Movie { get; } = default!;

    public Hall Hall { get; } = default!;

    public DateTime StartTime { get; private set; }

    public DateTime EndTime => StartTime.AddMinutes(Movie.Duration.Value);

    public IReadOnlyCollection<Booking> Bookings => _bookings.ToList().AsReadOnly();

    protected Session() { }

    public Session(Movie movie, Hall hall, DateTime startTime)
        : this(Guid.NewGuid(), movie, hall, startTime) { }

    protected Session(Guid id, Movie movie, Hall hall, DateTime startTime)
        : base(id)
    {
        Movie = movie ?? throw new ArgumentNullValueException(nameof(movie));
        Hall = hall ?? throw new ArgumentNullValueException(nameof(hall));
        StartTime = startTime;
    }

    public bool IsSeatAvailable(Seat seat)
    {
        if (seat == null) throw new ArgumentNullValueException(nameof(seat));

        return !_bookings
            .Where(b => b.IsActive())
            .SelectMany(b => b.Seats)
            .Any(s => s.Id == seat.Id);
    }

    public IReadOnlyCollection<Seat> GetAvailableSeats()
    {
        var bookedSeatIds = _bookings
            .Where(b => b.IsActive())
            .SelectMany(b => b.Seats)
            .Select(s => s.Id)
            .ToHashSet();

        return Hall.Seats.Where(s => !bookedSeatIds.Contains(s.Id)).ToList().AsReadOnly();
    }

    internal bool ChangeStartTime(DateTime newStartTime)
    {
        if (StartTime <= DateTime.UtcNow)
            throw new SessionAlreadyStartedException(this);

        if (StartTime == newStartTime) return false;
        StartTime = newStartTime;
        return true;
    }

    internal void AddBooking(Booking booking)
    {
        if (booking == null) throw new ArgumentNullValueException(nameof(booking));

        foreach (var seat in booking.Seats)
        {
            if (!IsSeatAvailable(seat))
                throw new SeatAlreadyBookedException(this, seat);
        }

        _bookings.Add(booking);
    }
}
