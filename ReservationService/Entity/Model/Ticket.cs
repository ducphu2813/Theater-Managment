using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ReservationService.Entity.Model;

public class Ticket
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    public string? MovieScheduleId { get; set; }
    public List<string>? SeatId { get; set; }
    public List<Seat>? SeatDetail  { get; set; }
    public List<string>? FoodId { get; set; }
    public List<Food>? FoodDetail { get; set; }
    public int TotalTicket { get; set; }
    public int TotalPrice { get; set; }
    public string? DiscountId { get; set; }
    public Discount? DiscountDetail { get; set; }
    public string? UserId { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedAt { get; set; }
}