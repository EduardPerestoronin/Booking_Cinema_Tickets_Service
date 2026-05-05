using Domain.ValueObject.Base;
using Domain.ValueObject.Validators;

namespace Domain.ValueObject;

public class RowNumber(int row) : ValueObject<int>(new SeatPositionValidator(), row);
