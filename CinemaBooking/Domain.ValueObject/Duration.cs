using Domain.ValueObject.Base;
using Domain.ValueObject.Validators;

namespace Domain.ValueObject;

public class Duration(int minutes) : ValueObject<int>(new DurationValidator(), minutes);
