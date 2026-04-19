namespace Domain.Cinema_Booking;

public class Hall
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public int TotalSeats { get; private set; }

    private readonly List<Seat> _seats = new();
    public IReadOnlyCollection<Seat> Seats => _seats;

    protected Hall() { }

    public Hall(Guid id, string name, int totalSeats)
    {
        Id = id;
        Name = string.IsNullOrWhiteSpace(name) ? "Hall" : name;
        TotalSeats = totalSeats > 0 ? totalSeats : throw new ArgumentException("Total seats must be positive");

        int seatsPerRow = 10;
        int rows = (int)Math.Ceiling((double)totalSeats / seatsPerRow);

        for (int row = 1; row <= rows; row++)
        {
            int seatsInThisRow = row == rows ? totalSeats - (rows - 1) * seatsPerRow : seatsPerRow;
            for (int seatNum = 1; seatNum <= seatsInThisRow; seatNum++)
            {
                _seats.Add(new Seat(Guid.NewGuid(), this, row, seatNum));
            }
        }
    }
}