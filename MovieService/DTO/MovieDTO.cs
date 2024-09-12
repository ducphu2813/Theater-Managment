namespace MovieService.DTO;

public class MovieDTO
{
    public string? Name { get; set; }
    public string? Director { get; set; }
    public string? Actors { get; set; }
    public string? Author { get; set; }
    public string? Description { get; set; }
    public string? Dub { get; set; }
    public string? SubTitle { get; set; }
    public List<string>? Genres { get; set; }
}