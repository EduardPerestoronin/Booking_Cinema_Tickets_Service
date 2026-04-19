using Domain.ValueObject.Base;
using Domain.ValueObject.Validators;

namespace Domain.ValueObject;

public class Email : ValueObject<string>
{
    public Email(string email) : base(new EmailValidator(), email) { }
}