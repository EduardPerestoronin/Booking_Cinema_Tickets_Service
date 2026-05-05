using Domain.ValueObject.Base;
using Domain.ValueObject.Validators;

namespace Domain.ValueObject;

public class HallName(string name) : ValueObject<string>(new HallNameValidator(), name);
