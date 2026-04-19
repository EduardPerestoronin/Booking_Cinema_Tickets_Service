using Domain.ValueObject.Base;
using Domain.ValueObject.Validators;

namespace Domain.ValueObject;

public class MovieTitle : ValueObject<string>
{
    public MovieTitle(string title) : base(new MovieTitleValidator(), title) { }
}