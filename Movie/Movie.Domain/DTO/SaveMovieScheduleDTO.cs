namespace Movie.Domain.DTO;

public class SaveMovieScheduleDTO
{
    public string? MovieId  { get; set; }
    
    public List<ScheduleDetail>? ScheduleDetails { get; set; }
}