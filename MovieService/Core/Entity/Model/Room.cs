using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MovieService.Core.Entity.Model;

public class Room : BaseModel
{
    
    public string? RoomNumber { get; set; }
    public int AvailableSeat { get; set; }
    public int SingleSeat { get; set; }
    public int CoupleSeat { get; set; }
}