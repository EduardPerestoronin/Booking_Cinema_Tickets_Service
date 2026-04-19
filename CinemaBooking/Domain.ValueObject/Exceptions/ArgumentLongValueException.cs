namespace Domain.ValueObject.Exceptions;

public class ArgumentLongValueException(string paramName, string value, int maxLength)
    : FormatException($"The \"{paramName}\" length {value.Length} greater than maximum allowed length {maxLength}")
{
    public string Value => value;
    public int MaxLength => maxLength;
}