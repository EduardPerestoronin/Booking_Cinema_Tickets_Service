using Domain.ValueObject.Base;
using Domain.ValueObject.Validators;

namespace Domain.ValueObject;

public class SeatNumber(int number) : ValueObject<int>(new SeatPositionValidator(), number);
