using Domain.ValueObject.Base;
using Domain.ValueObject.Validators;

namespace Domain.ValueObject;

public class MovieTitle(string title) : ValueObject<string>(new MovieTitleValidator(), title);
