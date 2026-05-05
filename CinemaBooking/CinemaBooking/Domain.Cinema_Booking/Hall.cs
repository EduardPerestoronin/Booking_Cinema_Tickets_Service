using Domain.Cinema_Booking.Base;
using Domain.Cinema_Booking.Exceptions;
using Domain.ValueObject;

namespace Domain.Cinema_Booking;

public class Hall : Entity<Guid>
{
 
    private readonly ICollection<Seat> _seats = [];

    public HallName Name { get; private set; } = default!;

    public SeatsCapacity TotalSeats { get; private set; } = default!;

    public IReadOnlyCollection<Seat> Seats => _seats.ToList().AsReadOnly();

    protected Hall() { }

    public Hall(HallName name, SeatsCapacity totalSeats)
        : this(Guid.NewGuid(), name, totalSeats) { }

    protected Hall(Guid id, HallName name, SeatsCapacity totalSeats)
        : base(id)
    {
        Name = name ?? throw new ArgumentNullValueException(nameof(name));
        TotalSeats = totalSeats ?? throw new ArgumentNullValueException(nameof(totalSeats));
    }

    public Seat AddSeat(RowNumber row, SeatNumber number)
    {
        if (row == null) throw new ArgumentNullValueException(nameof(row));
        if (number == null) throw new ArgumentNullValueException(nameof(number));

        var seat = new Seat(this, row, number);
        _seats.Add(seat);
        return seat;
    }

    public void GenerateDefaultSeats(int seatsPerRow = 10)
    {
        if (seatsPerRow <= 0)
            throw new ArgumentOutOfRangeException(nameof(seatsPerRow));

        int total = TotalSeats.Value;
        int rows = (int)Math.Ceiling((double)total / seatsPerRow);

        for (int row = 1; row <= rows; row++)
        {
            int seatsInThisRow = row == rows ? total - (rows - 1) * seatsPerRow : seatsPerRow;
            for (int n = 1; n <= seatsInThisRow; n++)
                AddSeat(new RowNumber(row), new SeatNumber(n));
        }
    }
}
