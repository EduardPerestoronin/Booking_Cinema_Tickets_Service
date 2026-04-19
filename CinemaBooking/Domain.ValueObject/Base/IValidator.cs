namespace Domain.ValueObject.Base;

public interface IValidator<T>
{
    void Validate(T value);
}