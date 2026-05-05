namespace Domain.ValueObject.Exceptions;

public class ArgumentNotPositiveException(string paramName, int value)
    : ArgumentOutOfRangeException(paramName, $"The \"{paramName}\" value {value} must be a positive integer.")
{
    public int Value => value;
}
