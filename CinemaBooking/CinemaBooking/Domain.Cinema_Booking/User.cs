using Domain.Cinema_Booking.Base;
using Domain.Cinema_Booking.Exceptions;
using Domain.ValueObject;

namespace Domain.Cinema_Booking;

public class User : Entity<Guid>
{

    private readonly ICollection<Booking> _bookings = [];

    public Username Username { get; private set; } = default!;

    public Email Email { get; private set; } = default!;

    public DateTime CreatedAt { get; }

    public IReadOnlyCollection<Booking> Bookings => _bookings.ToList().AsReadOnly();

    protected User() { }

    public User(Username username, Email email, DateTime createdAt)
        : this(Guid.NewGuid(), username, email, createdAt) { }

    protected User(Guid id, Username username, Email email, DateTime createdAt)
        : base(id)
    {
        Username = username ?? throw new ArgumentNullValueException(nameof(username));
        Email = email ?? throw new ArgumentNullValueException(nameof(email));
        CreatedAt = createdAt;
    }

    public IReadOnlyCollection<Movie> ViewMovies(IEnumerable<Movie> movies)
    {
        if (movies == null) throw new ArgumentNullValueException(nameof(movies));
        return movies.ToList().AsReadOnly();
    }

    public IReadOnlyCollection<Session> ViewSchedule(Movie movie, IEnumerable<Session> allSessions)
    {
        if (movie == null) throw new ArgumentNullValueException(nameof(movie));
        if (allSessions == null) throw new ArgumentNullValueException(nameof(allSessions));

        return allSessions
            .Where(s => s.Movie.Id == movie.Id)
            .ToList()
            .AsReadOnly();
    }

    public Session SelectSession(Session session)
    {
        if (session == null) throw new ArgumentNullValueException(nameof(session));
        if (session.StartTime <= DateTime.UtcNow)
            throw new SessionAlreadyStartedException(session);
        return session;
    }

    public IReadOnlyCollection<Seat> ViewSeats(Session session)
    {
        if (session == null) throw new ArgumentNullValueException(nameof(session));
        return session.Hall.Seats;
    }

    public Seat SelectSeat(Session session, Seat seat)
    {
        if (session == null) throw new ArgumentNullValueException(nameof(session));
        if (seat == null) throw new ArgumentNullValueException(nameof(seat));

        if (seat.Hall.Id != session.Hall.Id)
            throw new SeatNotInHallException(seat, session.Hall);

        if (!session.IsSeatAvailable(seat))
            throw new SeatAlreadyBookedException(session, seat);

        return seat;
    }

    public Booking CreateBooking(Session session, IEnumerable<Seat> seats, DateTime expiresAt)
    {
        if (session == null) throw new ArgumentNullValueException(nameof(session));
        if (seats == null) throw new ArgumentNullValueException(nameof(seats));

        if (session.StartTime <= DateTime.UtcNow)
            throw new SessionAlreadyStartedException(session);

        var booking = new Booking(this, session, seats, DateTime.UtcNow, expiresAt);
        session.AddBooking(booking);
        _bookings.Add(booking);
        return booking;
    }

    public void CancelBooking(Booking booking)
    {
        if (booking == null) throw new ArgumentNullValueException(nameof(booking));

        if (booking.User.Id != Id)
            throw new BookingNotBelongUserException(booking, this);

        if (!_bookings.Contains(booking))
            throw new BookingNotBelongUserException(booking, this);

        booking.Cancel();
    }

    internal bool ChangeUsername(Username newUsername)
    {
        if (newUsername == null) throw new ArgumentNullValueException(nameof(newUsername));
        if (Username == newUsername) return false;
        Username = newUsername;
        return true;
    }

    internal bool ChangeEmail(Email newEmail)
    {
        if (newEmail == null) throw new ArgumentNullValueException(nameof(newEmail));
        if (Email == newEmail) return false;
        Email = newEmail;
        return true;
    }
}
