using Domain.ValueObject.Base;
using Domain.ValueObject.Validators;

namespace Domain.ValueObject;

public class SeatsCapacity(int capacity) : ValueObject<int>(new SeatsCapacityValidator(), capacity);
