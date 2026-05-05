using Domain.Cinema_Booking.Base;
using Domain.Cinema_Booking.Exceptions;
using Domain.ValueObject;

namespace Domain.Cinema_Booking;

public class Seat : Entity<Guid>
{

    public Hall Hall { get; } = default!;

    public RowNumber Row { get; } = default!;

    public SeatNumber Number { get; } = default!;

    protected Seat() { }

    public Seat(Hall hall, RowNumber row, SeatNumber number)
        : this(Guid.NewGuid(), hall, row, number) { }

    protected Seat(Guid id, Hall hall, RowNumber row, SeatNumber number)
        : base(id)
    {
        Hall = hall ?? throw new ArgumentNullValueException(nameof(hall));
        Row = row ?? throw new ArgumentNullValueException(nameof(row));
        Number = number ?? throw new ArgumentNullValueException(nameof(number));
    }
}
