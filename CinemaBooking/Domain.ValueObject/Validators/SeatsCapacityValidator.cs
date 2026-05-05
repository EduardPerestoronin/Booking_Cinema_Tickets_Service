using Domain.ValueObject.Base;
using Domain.ValueObject.Exceptions;

namespace Domain.ValueObject.Validators;

public class SeatsCapacityValidator : IValidator<int>
{
    public static int MIN_CAPACITY => 1;
    public static int MAX_CAPACITY => 1000;

    public void Validate(int value)
    {
        if (value <= 0)
            throw new ArgumentNotPositiveException(nameof(value), value);

        if (value < MIN_CAPACITY || value > MAX_CAPACITY)
            throw new ArgumentOutOfBoundsException(nameof(value), value, MIN_CAPACITY, MAX_CAPACITY);
    }
}
