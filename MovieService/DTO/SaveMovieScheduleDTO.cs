namespace MovieService.DTO;

public class SaveMovieScheduleDTO
{
    public string? MovieId  { get; set; }
    
    public List<ScheduleDetail>? ScheduleDetails { get; set; }
}

public class ScheduleDetail
{
    public string? RoomNumber { get; set; }
    public DateTime? ShowTime { get; set; }
    public int? SingleSeatPrice { get; set; }
    public int? CoupleSeatPrice { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? Status { get; set; }
}