using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ReservationService.DTO;

namespace ReservationService.Entity.Model;

public class Ticket
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    public string? MovieScheduleId { get; set; }
    public string? RoomNumber { get; set; }
    public List<SeatDetailDTO>? SeatDetail  { get; set; }
    public int TotalTicket { get; set; }
    public List<FoodDetailDTO>? FoodDetail { get; set; }
    public int TotalPrice { get; set; }
    public List<DiscountDetailDTO>? DiscountDetail { get; set; }
    public string? UserId { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedAt { get; set; }
}