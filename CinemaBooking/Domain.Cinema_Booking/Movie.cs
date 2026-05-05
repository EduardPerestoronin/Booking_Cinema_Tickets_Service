using Domain.Cinema_Booking.Base;
using Domain.Cinema_Booking.Exceptions;
using Domain.ValueObject;

namespace Domain.Cinema_Booking;

public class Movie : Entity<Guid>
{
  
    public MovieTitle Title { get; private set; } = default!;

    public MovieDescription Description { get; private set; } = default!;

    public Duration Duration { get; private set; } = default!;

    public DateTime CreatedAt { get; }

    protected Movie() { }

    public Movie(MovieTitle title, MovieDescription description, Duration duration, DateTime createdAt)
        : this(Guid.NewGuid(), title, description, duration, createdAt) { }

    protected Movie(Guid id, MovieTitle title, MovieDescription description, Duration duration, DateTime createdAt)
        : base(id)
    {
        Title = title ?? throw new ArgumentNullValueException(nameof(title));
        Description = description ?? throw new ArgumentNullValueException(nameof(description));
        Duration = duration ?? throw new ArgumentNullValueException(nameof(duration));
        CreatedAt = createdAt;
    }

    internal bool ChangeTitle(MovieTitle newTitle)
    {
        if (newTitle == null) throw new ArgumentNullValueException(nameof(newTitle));
        if (Title == newTitle) return false;
        Title = newTitle;
        return true;
    }

    internal bool ChangeDescription(MovieDescription newDescription)
    {
        if (newDescription == null) throw new ArgumentNullValueException(nameof(newDescription));
        if (Description == newDescription) return false;
        Description = newDescription;
        return true;
    }

    internal bool ChangeDuration(Duration newDuration)
    {
        if (newDuration == null) throw new ArgumentNullValueException(nameof(newDuration));
        if (Duration == newDuration) return false;
        Duration = newDuration;
        return true;
    }
}
