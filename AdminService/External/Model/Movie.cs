namespace AdminService.External.Model;

public class Movie
{
    public string Id { get; set; }
    
    public required string Name { get; set; }
    public string? Director { get; set; }
    public string? Actors { get; set; }
    public string? Author { get; set; }
    public string? Description { get; set; }
    public string? Dub { get; set; }
    public string? SubTitle { get; set; }
}