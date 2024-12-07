using Movie.Domain.Entity;

namespace Movie.Domain.DTO;

public class MovieScheduleDTO
{
    public string? Id { get; set; }
    public movies? Movie  { get; set; }
    public string? RoomNumber { get; set; }
    public DateTime? ShowTime { get; set; }
    public int? SingleSeatPrice { get; set; }
    public int? CoupleSeatPrice { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? Status { get; set; }

    public MovieScheduleDTO()
    {
        
    }
    
    public MovieScheduleDTO(string id, movies movie, string roomId, DateTime showTime, int singleSeatPrice, int coupleSeatPrice, DateTime createdAt, string status)
    {
        Id = id;
        Movie = movie;
        RoomNumber = roomId;
        ShowTime = showTime;
        SingleSeatPrice = singleSeatPrice;
        CoupleSeatPrice = coupleSeatPrice;
        CreatedAt = createdAt;
        Status = status;
    }
    
    public MovieScheduleDTO(movies movie, string roomId, DateTime showTime, int singleSeatPrice, int coupleSeatPrice, DateTime createdAt, string status)
    {
        Movie = movie;
        RoomNumber = roomId;
        ShowTime = showTime;
        SingleSeatPrice = singleSeatPrice;
        CoupleSeatPrice = coupleSeatPrice;
        CreatedAt = createdAt;
        Status = status;
    }
    
    public MovieScheduleDTO(string roomId, DateTime showTime, int singleSeatPrice, int coupleSeatPrice, DateTime createdAt, string status)
    {
        RoomNumber = roomId;
        ShowTime = showTime;
        SingleSeatPrice = singleSeatPrice;
        CoupleSeatPrice = coupleSeatPrice;
        CreatedAt = createdAt;
        Status = status;
    }
}