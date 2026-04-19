namespace Domain.ValueObject.Exceptions;

public class ArgumentShortValueException(string paramName, string value, int minLength)
    : FormatException($"The \"{paramName}\" length {value.Length} less than minimum allowed length {minLength}")
{
    public string Value => value;
    public int MinLength => minLength;
}