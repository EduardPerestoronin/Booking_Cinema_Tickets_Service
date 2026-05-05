using Domain.ValueObject.Base;
using Domain.ValueObject.Validators;

namespace Domain.ValueObject;

public class Username(string username) : ValueObject<string>(new UsernameValidator(), username);
