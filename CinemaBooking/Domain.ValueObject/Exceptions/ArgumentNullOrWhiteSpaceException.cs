namespace Domain.ValueObject.Exceptions;

public class ArgumentNullOrWhiteSpaceException(string paramName)
    : ArgumentNullException(paramName, $"The \"{paramName}\" mustn't be null, empty or consists only of white-space characters.");