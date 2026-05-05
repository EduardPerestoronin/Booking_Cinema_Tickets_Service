using Domain.ValueObject.Base;
using Domain.ValueObject.Validators;

namespace Domain.ValueObject;

public class Email(string email) : ValueObject<string>(new EmailValidator(), email);
