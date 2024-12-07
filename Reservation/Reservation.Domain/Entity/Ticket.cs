using Shared.Entity;

namespace Reservation.Domain.Entity;


public class Ticket : BaseEntity
{
    
    public string? MovieScheduleId { get; set; }
    public List<string>? SeatId { get; set; }
    public List<Seat>? SeatDetail  { get; set; }
    public List<string>? FoodId { get; set; }
    public List<Food>? FoodDetail { get; set; }
    public float? BaseAmount { get; set; }
    public float? TotalAmount { get; set; }
    public string? DiscountId { get; set; }
    public Discount? DiscountDetail { get; set; }
    public string? UserId { get; set; }
    public string? Status { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? ExpiryTime { get; set; }
}