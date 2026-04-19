using Domain.ValueObject;

namespace Domain.Cinema_Booking;

public class Movie
{
    public Guid Id { get; private set; }
    public MovieTitle Title { get; private set; }
    public string Description { get; private set; }
    public int DurationMinutes { get; private set; }
    public DateTime CreatedAt { get; private set; }

    protected Movie() { }

    public Movie(Guid id, MovieTitle title, string description, int durationMinutes, DateTime createdAt)
    {
        Id = id;
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description ?? string.Empty;
        DurationMinutes = durationMinutes > 0 ? durationMinutes : throw new ArgumentException("Duration must be positive");
        CreatedAt = createdAt;
    }
}