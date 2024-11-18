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
    public int? Duration { get; set; }  //thời lượng phim tính bằng phút
    public string? Poster { get; set; }
}