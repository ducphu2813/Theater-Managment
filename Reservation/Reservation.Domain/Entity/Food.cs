using Shared.Entity;

namespace Reservation.Domain.Entity;

public class Food : BaseEntity
{
    public string? Name { get; set; }
    public string? FoodType { get; set; }
    public string? Description { get; set; }
    public int? Amount { get; set; }
    public string? Image { get; set; }
}