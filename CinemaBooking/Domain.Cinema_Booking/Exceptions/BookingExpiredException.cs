namespace Domain.Cinema_Booking.Exceptions;

public class BookingExpiredException(Guid bookingId, DateTime expiresAt)
    : InvalidOperationException($"Booking {bookingId} expired at {expiresAt}")
{
    public Guid BookingId => bookingId;
    public DateTime ExpiresAt => expiresAt;
}