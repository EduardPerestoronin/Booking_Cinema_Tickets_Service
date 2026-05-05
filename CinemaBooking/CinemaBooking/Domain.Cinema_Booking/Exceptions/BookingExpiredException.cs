namespace Domain.Cinema_Booking.Exceptions;

public class BookingExpiredException(Booking booking)
    : InvalidOperationException($"Booking {booking.Id} expired at {booking.ExpiresAt}.")
{
    public Booking Booking => booking;
}
