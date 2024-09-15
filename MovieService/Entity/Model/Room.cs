using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MovieService.Entity.Model;

public class Room
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? RoomNumber { get; set; }
    public int AvailableSeat { get; set; }
    public int SingleSeat { get; set; }
    public int CoupleSeat { get; set; }
}