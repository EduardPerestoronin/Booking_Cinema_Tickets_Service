using Domain.ValueObject.Base;
using Domain.ValueObject.Validators;

namespace Domain.ValueObject;

public class SeatNumber : ValueObject<string>
{
    public SeatNumber(string seatNumber) : base(new SeatNumberValidator(), seatNumber) { }

    public int Row => int.Parse(Value.Split('-')[0]);
    public int Number => int.Parse(Value.Split('-')[1]);
}