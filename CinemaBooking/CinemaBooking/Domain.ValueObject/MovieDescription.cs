using Domain.ValueObject.Base;
using Domain.ValueObject.Validators;

namespace Domain.ValueObject;

public class MovieDescription(string description) : ValueObject<string>(new MovieDescriptionValidator(), description);
