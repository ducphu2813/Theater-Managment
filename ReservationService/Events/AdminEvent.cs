using ReservationService.Entity.Model;
using ReservationService.External.Model;

namespace ReservationService.Events;

public class AdminEvent
{
    public string? UserId { get; set; }
    public string? TicketId { get; set; }
    public string? PaymentId { get; set; }
    public movies? MovieDetail { get; set; }
    public string? PaymentMethod { get; set; }
    public DateTime? ShowTime { get; set; }
    public float? BaseAmount { get; set; }
    public float? TotalAmount { get; set; }
    public List<String>? Genres { get; set; }
    public List<Seat>? SeatDetail { get; set; }
    public List<Food>? FoodDetail { get; set; }
    public Discount? DiscountDetail { get; set; }
    public DateTime? CreatedAt { get; set; }
}