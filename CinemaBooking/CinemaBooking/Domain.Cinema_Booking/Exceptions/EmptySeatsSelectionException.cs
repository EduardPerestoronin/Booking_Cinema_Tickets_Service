namespace Domain.Cinema_Booking.Exceptions;

public class EmptySeatsSelectionException()
    : InvalidOperationException("At least one seat must be selected to create a booking.");
