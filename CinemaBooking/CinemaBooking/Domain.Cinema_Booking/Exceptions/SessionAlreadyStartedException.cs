namespace Domain.Cinema_Booking.Exceptions;

public class SessionAlreadyStartedException(Session session)
    : InvalidOperationException($"Session {session.Id} already started at {session.StartTime}.")
{
    public Session Session => session;
}
