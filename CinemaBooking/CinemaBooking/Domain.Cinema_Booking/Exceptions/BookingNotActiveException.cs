namespace Domain.Cinema_Booking.Exceptions;

public class BookingNotActiveException(Booking booking)
    : InvalidOperationException($"Booking {booking.Id} is not active (current status: {booking.Status}).")
{
    public Booking Booking => booking;
}
