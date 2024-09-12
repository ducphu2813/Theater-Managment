namespace MovieService.DTO;

public class SaveMovieScheduleDTO
{
    public required string MovieId  { get; set; }
    public required string RoomNumber { get; set; }
    public required DateTime ShowTime { get; set; }
    public int SingleSeatPrice { get; set; }
    public int CoupleSeatPrice { get; set; }
    public required DateTime CreatedAt { get; set; }
    public string? Status { get; set; }
}