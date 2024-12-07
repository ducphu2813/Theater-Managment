using Analysis.Domain.ExternalModel;
using Shared.Entity;

namespace Analysis.Domain.Entity;

public class MovieSale : BaseEntity
{
    public string? UserId { get; set; }
    public string? TicketId { get; set; }
    public string? PaymentId { get; set; }
    public Movie? MovieDetail { get; set; }
    public string? PaymentMethod { get; set; }
    public DateTime? ShowTime { get; set; }
    public List<Seat>? SeatDetail { get; set; }
    public List<Food>? FoodDetail { get; set; }
    public Discount? DiscountDetail { get; set; }
    public float? BaseAmount { get; set; }
    public float? TotalAmount { get; set; }
    public List<String>? Genres { get; set; }
    public DateTime? TicketCreatedDate { get; set; }
}