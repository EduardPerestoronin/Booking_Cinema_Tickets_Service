using Domain.Cinema_Booking;
using Domain.ValueObject;

namespace DomainApp;

internal static class Program
{
    private static readonly List<Movie> _movies = [];
    private static readonly List<Hall> _halls = [];
    private static readonly List<Session> _sessions = [];
    private static readonly List<Booking> _bookings = [];

    private static User _user = null!;
    private static Administrator _admin = null!;

    private static void Main() 
    {
        Seed();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("КИНОТЕАТР");
            Console.WriteLine("1. Войти как ПОЛЬЗОВАТЕЛЬ");
            Console.WriteLine("2. Войти как АДМИНИСТРАТОР");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите роль: ");

            var role = Console.ReadLine();
            try
            {
                switch (role)
                {
                    case "1": UserMenu(); break;
                    case "2": AdminMenu(); break;
                    case "0": return;
                    default: Console.WriteLine("Неверный выбор"); Pause(); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                Pause();
            }
        }
    }

    private static void UserMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("ПОЛЬЗОВАТЕЛЬ");
            Console.WriteLine($"Вы вошли как: {_user.Username}");
            Console.WriteLine();
            Console.WriteLine("1. Просмотреть фильмы");
            Console.WriteLine("2. Просмотреть расписание");
            Console.WriteLine("3. Выбрать сеанс и забронировать билет");
            Console.WriteLine("4. Просмотреть свои бронирования");
            Console.WriteLine("5. Отменить бронирование");
            Console.WriteLine("0. Назад");
            Console.Write("Действие: ");

            var action = Console.ReadLine();
            try
            {
                switch (action)
                {
                    case "1": ShowMovies(); break;
                    case "2": ShowSchedule(); break;
                    case "3": BookSeat(); break;
                    case "4": ShowMyBookings(); break;
                    case "5": CancelBooking(); break;
                    case "0": return;
                    default: Console.WriteLine("Неверный выбор"); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            Pause();
        }
    }

    private static void ShowMovies()
    {
        var movies = _user.ViewMovies(_movies);
        Console.WriteLine("\nФильмы");
        int i = 1;
        foreach (var m in movies)
            Console.WriteLine($"{i++}. {m.Title} ({m.Duration.Value} мин.) — {m.Description}");
    }

    private static void ShowSchedule()
    {
        var movie = ChooseMovie();
        if (movie == null) return;

        var sessions = _user.ViewSchedule(movie, _sessions);
        Console.WriteLine($"\nРасписание для «{movie.Title}»");
        if (sessions.Count == 0) { Console.WriteLine("Нет сеансов"); return; }
        int i = 1;
        foreach (var s in sessions)
            Console.WriteLine($"{i++}. {s.StartTime:dd.MM HH:mm} — {s.Hall.Name}");
    }

    private static void BookSeat()
    {
        var session = ChooseSession();
        if (session == null) return;

        _user.SelectSession(session);

        var seats = _user.ViewSeats(session);
        var available = session.GetAvailableSeats();

        Console.WriteLine("\nМеста (свободные отмечены *)");
        var availableSet = available.Select(s => s.Id).ToHashSet();
        var list = seats.OrderBy(s => s.Row.Value).ThenBy(s => s.Number.Value).ToList();
        for (int i = 0; i < list.Count; i++)
        {
            var s = list[i];
            var mark = availableSet.Contains(s.Id) ? "*" : " ";
            Console.Write($"[{i + 1,2}{mark}]ряд {s.Row.Value} место {s.Number.Value}   ");
            if ((i + 1) % 4 == 0) Console.WriteLine();
        }
        Console.WriteLine();

        Console.Write("Введите номера мест через запятую: ");
        var input = Console.ReadLine() ?? string.Empty;
        var indexes = input.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                           .Select(x => int.TryParse(x, out var n) ? n : -1)
                           .Where(n => n >= 1 && n <= list.Count)
                           .Distinct()
                           .ToList();

        if (indexes.Count == 0) { Console.WriteLine("Места не выбраны"); return; }

        var chosenSeats = indexes.Select(i => _user.SelectSeat(session, list[i - 1])).ToList();

        var booking = _user.CreateBooking(session, chosenSeats, DateTime.UtcNow.AddMinutes(15));
        _bookings.Add(booking);

        Console.WriteLine($"\nБронь создана: {booking.Id.ToString()[..8]} — {booking.Seats.Count} мест(а), статус: {booking.Status}");
        Console.WriteLine("Подтвердить бронь? (да/нет)");
        if ((Console.ReadLine() ?? "").Trim().ToLower() == "да")
        {
            booking.Confirm();
            Console.WriteLine($"Статус: {booking.Status}");
        }
    }

    private static void ShowMyBookings()
    {
        Console.WriteLine("\nМои бронирования");
        if (_user.Bookings.Count == 0) { Console.WriteLine("Нет бронирований"); return; }
        int i = 1;
        foreach (var b in _user.Bookings)
        {
            var seats = string.Join(", ", b.Seats.Select(s => $"{s.Row.Value}-{s.Number.Value}"));
            Console.WriteLine($"{i++}. {b.Id.ToString()[..8]} | {b.Session.Movie.Title} | {b.Session.StartTime:dd.MM HH:mm} | места: {seats} | {b.Status}");
        }
    }

    private static void CancelBooking()
    {
        if (_user.Bookings.Count == 0) { Console.WriteLine("Нет бронирований"); return; }
        ShowMyBookings();
        Console.Write("Номер для отмены: ");
        if (!int.TryParse(Console.ReadLine(), out var idx) || idx < 1 || idx > _user.Bookings.Count)
        { Console.WriteLine("Неверный номер"); return; }

        var booking = _user.Bookings.ElementAt(idx - 1);
        _user.CancelBooking(booking);
        Console.WriteLine($"Бронь отменена. Статус: {booking.Status}");
    }

    private static void AdminMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("АДМИНИСТРАТОР");
            Console.WriteLine($"Вы вошли как: {_admin.Username}");
            Console.WriteLine();
            Console.WriteLine("1. Добавить фильм");
            Console.WriteLine("2. Добавить сеанс");
            Console.WriteLine("3. Обновить сеанс (время начала)");
            Console.WriteLine("4. Удалить сеанс");
            Console.WriteLine("5. Просмотреть бронирования");
            Console.WriteLine("0. Назад");
            Console.Write("Действие: ");

            var action = Console.ReadLine();
            try
            {
                switch (action)
                {
                    case "1": AddMovie(); break;
                    case "2": AddSession(); break;
                    case "3": UpdateSession(); break;
                    case "4": DeleteSession(); break;
                    case "5": ShowAllBookings(); break;
                    case "0": return;
                    default: Console.WriteLine("Неверный выбор"); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            Pause();
        }
    }

    private static void AddMovie()
    {
        Console.Write("Название: ");
        var title = Console.ReadLine() ?? "";
        Console.Write("Описание: ");
        var desc = Console.ReadLine() ?? "";
        Console.Write("Длительность (мин): ");
        if (!int.TryParse(Console.ReadLine(), out var dur)) { Console.WriteLine("Неверная длительность"); return; }

        var movie = _admin.AddMovie(new MovieTitle(title), new MovieDescription(desc), new Duration(dur));
        _movies.Add(movie);
        Console.WriteLine($"Фильм «{movie.Title}» добавлен");
    }

    private static void AddSession()
    {
        var movie = ChooseMovie();
        if (movie == null) return;
        var hall = ChooseHall();
        if (hall == null) return;

        Console.Write("Дата и время начала (yyyy-MM-dd HH:mm): ");
        if (!DateTime.TryParse(Console.ReadLine(), out var start))
        { Console.WriteLine("Неверная дата"); return; }

        var session = _admin.AddSession(movie, hall, start);
        _sessions.Add(session);
        Console.WriteLine($"Сеанс «{movie.Title}» в {hall.Name} на {session.StartTime:dd.MM HH:mm} добавлен");
    }

    private static void UpdateSession()
    {
        var session = ChooseSession();
        if (session == null) return;

        Console.Write("Новое время (yyyy-MM-dd HH:mm): ");
        if (!DateTime.TryParse(Console.ReadLine(), out var newTime))
        { Console.WriteLine("Неверная дата"); return; }

        var changed = _admin.UpdateSession(session, newTime);
        Console.WriteLine(changed ? $"Сеанс перенесён на {session.StartTime:dd.MM HH:mm}" : "Время не изменилось");
    }

    private static void DeleteSession()
    {
        var session = ChooseSession();
        if (session == null) return;

        _admin.DeleteSession(session, _sessions);
        Console.WriteLine("Сеанс удалён");
    }

    private static void ShowAllBookings()
    {
        var bookings = _admin.ViewBookings(_bookings);
        Console.WriteLine("\nВсе бронирования");
        if (bookings.Count == 0) { Console.WriteLine("Бронирований нет"); return; }
        int i = 1;
        foreach (var b in bookings)
        {
            var seats = string.Join(", ", b.Seats.Select(s => $"{s.Row.Value}-{s.Number.Value}"));
            Console.WriteLine($"{i++}. {b.Id.ToString()[..8]} | {b.User.Username} | {b.Session.Movie.Title} | {b.Session.StartTime:dd.MM HH:mm} | места: {seats} | {b.Status}");
        }
    }

    private static Movie? ChooseMovie()
    {
        if (_movies.Count == 0) { Console.WriteLine("Фильмов нет"); return null; }
        Console.WriteLine("\nФильмы");
        for (int i = 0; i < _movies.Count; i++)
            Console.WriteLine($"{i + 1}. {_movies[i].Title}");
        Console.Write("Номер фильма: ");
        if (!int.TryParse(Console.ReadLine(), out var idx) || idx < 1 || idx > _movies.Count) return null;
        return _movies[idx - 1];
    }

    private static Hall? ChooseHall()
    {
        if (_halls.Count == 0) { Console.WriteLine("Залов нет"); return null; }
        Console.WriteLine("\nЗалы");
        for (int i = 0; i < _halls.Count; i++)
            Console.WriteLine($"{i + 1}. {_halls[i].Name} ({_halls[i].TotalSeats.Value} мест)");
        Console.Write("Номер зала: ");
        if (!int.TryParse(Console.ReadLine(), out var idx) || idx < 1 || idx > _halls.Count) return null;
        return _halls[idx - 1];
    }

    private static Session? ChooseSession()
    {
        if (_sessions.Count == 0) { Console.WriteLine("Сеансов нет"); return null; }
        Console.WriteLine("\nСеансы");
        for (int i = 0; i < _sessions.Count; i++)
        {
            var s = _sessions[i];
            Console.WriteLine($"{i + 1}. {s.Movie.Title} | {s.Hall.Name} | {s.StartTime:dd.MM HH:mm}");
        }
        Console.Write("Номер сеанса: ");
        if (!int.TryParse(Console.ReadLine(), out var idx) || idx < 1 || idx > _sessions.Count) return null;
        return _sessions[idx - 1];
    }

    private static void Pause()
    {
        Console.WriteLine("\nНажмите Enter для продолжения...");
        try
        {
            if (!Console.IsInputRedirected)
                Console.ReadKey();
            else
                Console.ReadLine();
        }
        catch (InvalidOperationException)
        {
            Console.ReadLine();
        }
    }

    private static void Seed()
    {
        _user = new User(new Username("Алексей"), new Email("alex@mail.ru"), DateTime.UtcNow);
        _admin = new Administrator(new Username("Админ"), new Email("admin@mail.ru"), DateTime.UtcNow);

        var movie1 = new Movie(new MovieTitle("Аватар"), new MovieDescription("Фантастика"), new Duration(162), DateTime.UtcNow);
        var movie2 = new Movie(new MovieTitle("Дюна 2"), new MovieDescription("Фантастика, продолжение"), new Duration(166), DateTime.UtcNow);
        _movies.Add(movie1);
        _movies.Add(movie2);

        var hall1 = new Hall(new HallName("Зал 1"), new SeatsCapacity(30));
        hall1.GenerateDefaultSeats(seatsPerRow: 10);
        var hall2 = new Hall(new HallName("Зал 2 VIP"), new SeatsCapacity(20));
        hall2.GenerateDefaultSeats(seatsPerRow: 5);
        _halls.Add(hall1);
        _halls.Add(hall2);

        _sessions.Add(new Session(movie1, hall1, DateTime.Now.AddHours(2)));
        _sessions.Add(new Session(movie2, hall2, DateTime.Now.AddHours(4)));
    }
}
