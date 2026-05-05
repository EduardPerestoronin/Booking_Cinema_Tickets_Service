namespace Domain.Cinema_Booking.Exceptions;

public class SeatAlreadyBookedException(Session session, Seat seat)
    : InvalidOperationException($"Seat (row {seat.Row.Value}, number {seat.Number.Value}) is already booked for session {session.Id}.")
{
    public Session Session => session;
    public Seat Seat => seat;
}
