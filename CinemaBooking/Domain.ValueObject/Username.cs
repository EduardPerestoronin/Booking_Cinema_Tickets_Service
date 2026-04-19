using Domain.ValueObject.Base;
using Domain.ValueObject.Validators;

namespace Domain.ValueObject;

public class Username : ValueObject<string>
{
    public Username(string username) : base(new UsernameValidator(), username) { }
}