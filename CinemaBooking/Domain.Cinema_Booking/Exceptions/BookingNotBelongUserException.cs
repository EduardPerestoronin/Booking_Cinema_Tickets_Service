namespace Domain.Cinema_Booking.Exceptions;

public class BookingNotBelongUserException(Booking booking, User user)
    : InvalidOperationException($"Booking {booking.Id} does not belong to user {user.Username} (user id = {user.Id}).")
{
    public Booking Booking => booking;
    public User User => user;
}
