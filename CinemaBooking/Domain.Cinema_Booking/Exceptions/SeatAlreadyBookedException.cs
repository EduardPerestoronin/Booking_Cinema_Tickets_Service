namespace Domain.Cinema_Booking.Exceptions;

public class SeatAlreadyBookedException(Guid sessionId, string seatNumber)
    : InvalidOperationException($"Seat {seatNumber} is already booked for session {sessionId}")
{
    public Guid SessionId => sessionId;
    public string SeatNumber => seatNumber;
}