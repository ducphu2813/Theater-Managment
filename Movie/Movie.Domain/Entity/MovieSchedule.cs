using Shared.Entity;

namespace Movie.Domain.Entity;

public class MovieSchedule : BaseEntity
{
    
    public string? MovieId { get; set; }
    public string? RoomNumber { get; set; }
    public DateTime? ShowTime { get; set; }
    public int? Duration { get; set; } //thời lượng phim tính bằng phút
    public int? SingleSeatPrice { get; set; }
    public int? CoupleSeatPrice { get; set; }
    public DateTime? CreatedAt { get; set; }
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