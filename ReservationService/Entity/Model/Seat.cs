using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ReservationService.Entity.Model;

public class Seat
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    public string? Row { get; set; }
    public string? Column { get; set; }
    public string? SeatType { get; set; }
}