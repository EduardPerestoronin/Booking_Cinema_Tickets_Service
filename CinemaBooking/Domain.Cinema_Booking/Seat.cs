using Domain.ValueObject;

namespace Domain.Cinema_Booking;

public class Seat
{
    public Guid Id { get; private set; }
    public Hall Hall { get; private set; }
    public int Row { get; private set; }
    public int Number { get; private set; }

    public SeatNumber SeatNumber => new SeatNumber($"{Row}-{Number}");

    protected Seat() { }

    public Seat(Guid id, Hall hall, int row, int number)
    {
        Id = id;
        Hall = hall ?? throw new ArgumentNullException(nameof(hall));
        Row = row;
        Number = number;
    }
}