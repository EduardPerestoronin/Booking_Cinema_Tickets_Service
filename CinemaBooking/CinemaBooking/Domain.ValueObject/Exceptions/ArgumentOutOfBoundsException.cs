namespace Domain.ValueObject.Exceptions;

public class ArgumentOutOfBoundsException(string paramName, int value, int min, int max)
    : ArgumentOutOfRangeException(paramName, $"The \"{paramName}\" value {value} is out of allowed range [{min}; {max}].")
{
    public int Value => value;
    public int Min => min;
    public int Max => max;
}
