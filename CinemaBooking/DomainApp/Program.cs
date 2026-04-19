using Domain.Cinema_Booking;
using Domain.ValueObject;

namespace DomainApp;

class Program
{
    static void Main()
    {
        var user = new User(Guid.NewGuid(), new Username("Алексей"), new Email("alex@mail.ru"), DateTime.UtcNow);
        var admin = new User(Guid.NewGuid(), new Username("Админ"), new Email("admin@mail.ru"), DateTime.UtcNow);
        var movie = new Movie(Guid.NewGuid(), new MovieTitle("Аватар"), "Фантастика", 162, DateTime.UtcNow);
        var hall = new Hall(Guid.NewGuid(), "Зал 1", 30);
        var session = new Session(Guid.NewGuid(), movie, hall, DateTime.Now.AddHours(1), DateTime.Now.AddHours(3).AddMinutes(42));

        Console.WriteLine("=== КИНОТЕАТР ===");
        Console.WriteLine("1. Войти как ПОЛЬЗОВАТЕЛЬ");
        Console.WriteLine("2. Войти как АДМИНИСТРАТОР");
        Console.Write("Выберите роль: ");
        var role = Console.ReadLine();

        if (role == "1")
        {
            // ПОЛЬЗОВАТЕЛЬ
            Console.Clear();
            Console.WriteLine("=== ПОЛЬЗОВАТЕЛЬ ===");
            Console.WriteLine("1. Просмотреть фильмы: Аватар");
            Console.WriteLine("2. Просмотреть расписание: Аватар - " + session.StartTime.ToString("HH:mm"));
            Console.WriteLine("3. Выбрать сеанс: Аватар в " + session.StartTime.ToString("HH:mm"));
            Console.WriteLine("4. Просмотреть места: 1-1, 1-2, 1-3, 2-1, 2-2, 2-3");

            var seat = hall.Seats.First(s => s.Row == 1 && s.Number == 3);
            Console.WriteLine($"5. Выбрать место: {seat.SeatNumber.Value}");

            var booking = session.BookSeat(user, seat, DateTime.UtcNow.AddMinutes(10));
            Console.WriteLine($"6. Забронировать билет: ID {booking.Id.ToString().Substring(0, 8)}");

            Console.WriteLine("7. Отменить бронь? (да/нет)");
            var answer = Console.ReadLine();
            if (answer == "да")
            {
                booking.Cancel();
                Console.WriteLine("Бронь отменена");
            }
            else
            {
                booking.Confirm();
                Console.WriteLine("Бронь подтверждена");
            }
        }
        else if (role == "2")
        {
            // АДМИНИСТРАТОР
            Console.Clear();
            Console.WriteLine("=== АДМИНИСТРАТОР ===");
            Console.WriteLine("1. Добавить фильм");
            Console.WriteLine("2. Добавить сеанс");
            Console.WriteLine("3. Обновить сеанс");
            Console.WriteLine("4. Удалить сеанс");
            Console.WriteLine("5. Просмотреть бронирования");
            Console.Write("Выберите действие: ");
            var action = Console.ReadLine();

            if (action == "1")
            {
                Console.WriteLine("Фильм 'Дюна 2' добавлен");
            }
            else if (action == "2")
            {
                Console.WriteLine("Сеанс 'Дюна 2' добавлен на завтра 19:00");
            }
            else if (action == "3")
            {
                Console.WriteLine("Сеанс 'Дюна 2' обновлён, перенесён на 20:00");
            }
            else if (action == "4")
            {
                Console.WriteLine("Сеанс 'Дюна 2' удалён");
            }
            else if (action == "5")
            {
                Console.WriteLine("Бронирования: пока нет");
            }
        }

        Console.WriteLine("\nНажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
}