using Domain.ValueObject.Base;
using Domain.ValueObject.Exceptions;

namespace Domain.ValueObject.Validators;

public class DurationValidator : IValidator<int>
{
    public static int MIN_MINUTES => 1;
    public static int MAX_MINUTES => 600;

    public void Validate(int value)
    {
        if (value <= 0)
            throw new ArgumentNotPositiveException(nameof(value), value);

        if (value < MIN_MINUTES || value > MAX_MINUTES)
            throw new ArgumentOutOfBoundsException(nameof(value), value, MIN_MINUTES, MAX_MINUTES);
    }
}
