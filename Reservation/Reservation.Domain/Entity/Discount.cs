using Shared.Entity;

namespace Reservation.Domain.Entity;

public class Discount : BaseEntity
{
    
    public string? FoodType { get; set; }
    public string? SeatType { get; set; }
    public int? PercentOff { get; set; }
}