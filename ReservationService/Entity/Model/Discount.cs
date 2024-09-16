using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ReservationService.Entity.Model;

public class Discount
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    public string? FoodType { get; set; }
    public string? SeatType { get; set; }
    public int? PercentOff { get; set; }
}