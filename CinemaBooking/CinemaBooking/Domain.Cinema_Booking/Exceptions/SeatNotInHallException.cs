namespace Domain.Cinema_Booking.Exceptions;

public class SeatNotInHallException(Seat seat, Hall hall)
    : InvalidOperationException($"Seat (row {seat.Row.Value}, number {seat.Number.Value}) does not belong to hall {hall.Name} (hall id = {hall.Id}).")
{
    public Seat Seat => seat;
    public Hall Hall => hall;
}
