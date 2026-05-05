namespace Domain.Cinema_Booking.Exceptions;

public class InvalidSessionTimeException(DateTime startTime)
    : ArgumentException($"Session start time {startTime} is not valid (must be in the future).")
{
    public DateTime StartTime => startTime;
}
