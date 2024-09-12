
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MovieService.Entity.Model;

public class MovieSchedule
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    public string? MovieId { get; set; }
    public required string RoomNumber { get; set; }
    public required DateTime ShowTime { get; set; }
    public int SingleSeatPrice { get; set; }
    public int CoupleSeatPrice { get; set; }
    public required DateTime CreatedAt { get; set; }
    public string? Status { get; set; }
    
    public MovieSchedule()
    {
        
    }
    
    public MovieSchedule(string movieId, string roomId, DateTime showTime, int singleSeatPrice, int coupleSeatPrice, DateTime createdAt, string status)
    {
        MovieId = movieId;
        RoomNumber = roomId;
        ShowTime = showTime;
        SingleSeatPrice = singleSeatPrice;
        CoupleSeatPrice = coupleSeatPrice;
        CreatedAt = createdAt;
        Status = status;
    }
    
    public MovieSchedule(string id, string movieId, string roomId, DateTime showTime, int singleSeatPrice, int coupleSeatPrice, DateTime createdAt, string status)
    {
        Id = id;
        MovieId = movieId;
        RoomNumber = roomId;
        ShowTime = showTime;
        SingleSeatPrice = singleSeatPrice;
        CoupleSeatPrice = coupleSeatPrice;
        CreatedAt = createdAt;
        Status = status;
    }
    
    public MovieSchedule(string roomId, DateTime showTime, int singleSeatPrice, int coupleSeatPrice, DateTime createdAt, string status)
    {
        RoomNumber = roomId;
        ShowTime = showTime;
        SingleSeatPrice = singleSeatPrice;
        CoupleSeatPrice = coupleSeatPrice;
        CreatedAt = createdAt;
        Status = status;
    }
}