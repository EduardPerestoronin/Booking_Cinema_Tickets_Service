using Domain.Cinema_Booking.Exceptions;
using Domain.ValueObject;

namespace Domain.Cinema_Booking;
public class Administrator : User
{
    protected Administrator() { }

    public Administrator(Username username, Email email, DateTime createdAt)
        : base(username, email, createdAt) { }

    protected Administrator(Guid id, Username username, Email email, DateTime createdAt)
        : base(id, username, email, createdAt) { }
    public Movie AddMovie(MovieTitle title, MovieDescription description, Duration duration)
    {
        if (title == null) throw new ArgumentNullValueException(nameof(title));
        if (description == null) throw new ArgumentNullValueException(nameof(description));
        if (duration == null) throw new ArgumentNullValueException(nameof(duration));

        return new Movie(title, description, duration, DateTime.UtcNow);
    }
    public Session AddSession(Movie movie, Hall hall, DateTime startTime)
    {
        if (movie == null) throw new ArgumentNullValueException(nameof(movie));
        if (hall == null) throw new ArgumentNullValueException(nameof(hall));

        if (startTime <= DateTime.UtcNow)
            throw new InvalidSessionTimeException(startTime);

        return new Session(movie, hall, startTime);
    }

    public bool UpdateSession(Session session, DateTime newStartTime)
    {
        if (session == null) throw new ArgumentNullValueException(nameof(session));
        if (newStartTime <= DateTime.UtcNow)
            throw new InvalidSessionTimeException(newStartTime);

        return session.ChangeStartTime(newStartTime);
    }

    public void DeleteSession(Session session, ICollection<Session> sessions)
    {
        if (session == null) throw new ArgumentNullValueException(nameof(session));
        if (sessions == null) throw new ArgumentNullValueException(nameof(sessions));

        if (session.StartTime <= DateTime.UtcNow)
            throw new SessionAlreadyStartedException(session);

        if (session.Bookings.Any(b => b.IsActive()))
            throw new InvalidOperationException(
                $"Cannot delete session {session.Id}: it has active bookings.");

        sessions.Remove(session);
    }

    public IReadOnlyCollection<Booking> ViewBookings(IEnumerable<Booking> allBookings, Session? session = null)
    {
        if (allBookings == null) throw new ArgumentNullValueException(nameof(allBookings));

        var bookings = session is null
            ? allBookings
            : allBookings.Where(b => b.Session.Id == session.Id);

        return bookings.ToList().AsReadOnly();
    }
}
