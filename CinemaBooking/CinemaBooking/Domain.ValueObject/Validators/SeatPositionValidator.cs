using Domain.ValueObject.Base;
using Domain.ValueObject.Exceptions;

namespace Domain.ValueObject.Validators;

public class SeatPositionValidator : IValidator<int>
{
    public static int MIN_VALUE => 1;
    public static int MAX_VALUE => 100;

    public void Validate(int value)
    {
        if (value <= 0)
            throw new ArgumentNotPositiveException(nameof(value), value);

        if (value < MIN_VALUE || value > MAX_VALUE)
            throw new ArgumentOutOfBoundsException(nameof(value), value, MIN_VALUE, MAX_VALUE);
    }
}
