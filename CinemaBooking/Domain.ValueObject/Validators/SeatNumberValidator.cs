using Domain.ValueObject.Base;
using Domain.ValueObject.Exceptions;

namespace Domain.ValueObject.Validators;

public class SeatNumberValidator : IValidator<string>
{
    public static int MAX_ROW => 20;
    public static int MAX_SEAT => 30;

    public void Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullOrWhiteSpaceException(nameof(value));

        var parts = value.Split('-');
        if (parts.Length != 2)
            throw new FormatException("Seat number must be in format 'row-seat', e.g. '5-12'");

        if (!int.TryParse(parts[0], out int row) || !int.TryParse(parts[1], out int seat))
            throw new FormatException("Row and seat must be numbers");

        if (row < 1 || row > MAX_ROW)
            throw new ArgumentOutOfRangeException($"Row must be between 1 and {MAX_ROW}");

        if (seat < 1 || seat > MAX_SEAT)
            throw new ArgumentOutOfRangeException($"Seat must be between 1 and {MAX_SEAT}");
    }
}