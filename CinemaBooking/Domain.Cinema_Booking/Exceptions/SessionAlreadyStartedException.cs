namespace Domain.Cinema_Booking.Exceptions;

public class SessionAlreadyStartedException(Guid sessionId, DateTime startTime)
    : InvalidOperationException($"Session {sessionId} already started at {startTime}")
{
    public Guid SessionId => sessionId;
    public DateTime StartTime => startTime;
}